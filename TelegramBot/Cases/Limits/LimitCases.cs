using DataInteraction;
using DataInteraction.Models;

namespace TelegramBot.Cases.Limits
{
    public partial class LimitCases : BaseCase
    {
        private DbProxy _dbProxy;

        public LimitCases(DbProxy dbProxy)
        {
            _dbProxy = dbProxy;
        }

        protected override Func<string, List<string>, string> GetNextStep(string userName, List<string> commands)
        {
            if (commands.Count == 1)
                return GetFirstMessage;

            if (commands.Count > 1)
            {
                if (commands[1] == LimitKeyPhrases.AddLimitType)
                    return GetNextStep_AddLimitType(userName, commands);

                if (commands[1] == LimitKeyPhrases.ChangeLimitType)
                    return GetNextStep_ChangeLimitType(userName, commands);
                // TODO: Забыл про ввод задолжности
                if (commands[1] == LimitKeyPhrases.AddLimit)
                    return GetNextStep_AddLimit(commands);
            }

            return DefaultAnswer;
        }

        #region MainActions



        private bool AddLimit(long currencyId, double limitSumm, long LimitType,
            DateTime startPeriod, DateTime endPeriod, double creditByLastPeriod = 0.0)
        {
            try
            {
                _dbProxy.InsertLimit(new Limit()
                {
                    CurrencyId = currencyId,
                    StartPeriod = startPeriod,
                    EndPeriod = endPeriod,
                    LimitSumm = limitSumm,
                    LimitTypeId = LimitType,
                    CreditByLastPeriod = creditByLastPeriod

                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private void AddLimitType()
        {
            throw new NotImplementedException();
        }

        private void ChangeLimit()
        {

        }

        private bool UpdateLimit(long limitId, long currencyId, double limitSumm, long LimitType,
            DateTime startPeriod, DateTime endPeriod, double creditByLastPeriod = 0.0)
        {
            try
            {
                _dbProxy.UpdateLimit(new Limit()
                {
                    ID = limitId,
                    CurrencyId = currencyId,
                    StartPeriod = startPeriod,
                    EndPeriod = endPeriod,
                    LimitSumm = limitSumm,
                    LimitTypeId = LimitType,
                    CreditByLastPeriod = creditByLastPeriod

                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        #endregion

        #region InteractionActions
        
        private string GetFirstMessage(string userName, List<string> commands)
        {
            return LimitKeyPhrases.ChoiceActionMessage;
        } 

        private Func<string, List<string>, string> GetNextStep_AddLimit(List<string> commands)
        {
            switch (commands.Count)
            {
                case 2:
                    // Выбор начала периода лимита
                    return AddLimit_ChoiceLimitStartDate;
                case 3:
                    // Выбор валюты
                    return AddLimit_ChoiceCurrency;
                case 4:
                    // Выбор размера лимита
                    return AddLimit_ChoiceLimitSize;
                case 5:
                    // Выбор типа лимита
                    return AddLimit_ChoiceLimitType;
                case 6:
                    {
                        // Не Custom лимит
                        if (commands[5] != "/0")
                            return AddLimit_DoItAndreturnCompleteMessage;
                        // Для Custom дату задаёт сам пользователь
                        else
                            return AddLimit_ChoiceLimitEndDate;
                    }
                case 7:
                    // Получены данные о дате окончания лимита для кастомного типа
                    return AddLimit_DoItAndreturnCompleteMessage;

                default:
                    return DefaultAnswer;
            }
        }

        private Func<string, List<string>, string> GetNextStep_AddLimitType(string userName, List<string> commands)
        {
            return DefaultAnswer;
        }

        private Func<string, List<string>, string> GetNextStep_ChangeLimitType(string userName, List<string> commands)
        {
            return DefaultAnswer;
        }

        #endregion

        #region HelpMethod

        public void RemoveLastCommand(List<string> commands)
        {
            commands.RemoveAt(commands.FindLastIndex(it => it.Length != 0));
        }

        #endregion

    }
}
