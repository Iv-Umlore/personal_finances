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
            switch (commands.Count)
            {
                case 1:
                    return GetFirstMessage;

                case 2:
                    if (commands[1] == LimitKeyPhrases.AddLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.ChangeLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.AddLimit)
                    {
                        // Выбор начала периода лимита
                        return AddLimit_ChoiceLimitStartDate;
                    }
                    return DefaultAnswer;

                case 3:
                    if (commands[1] == LimitKeyPhrases.AddLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.ChangeLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.AddLimit)
                    {
                        // Выбор валюты
                        return AddLimit_ChoiceCurrency;
                    }
                    return DefaultAnswer;

                case 4:
                    if (commands[1] == LimitKeyPhrases.AddLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.ChangeLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.AddLimit)
                    {
                        // Выбор размера лимита
                        return AddLimit_ChoiceLimitSize;
                    }
                    return DefaultAnswer;

                case 5:
                    if (commands[1] == LimitKeyPhrases.AddLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.ChangeLimitType)
                    {

                    }
                    if (commands[1] == LimitKeyPhrases.AddLimit)
                    {
                        // Выбор типа лимита
                        return AddLimit_ChoiceLimitType;
                    }
                    return DefaultAnswer;

                case 6:
                    if (commands[1] == LimitKeyPhrases.AddLimit)
                    {
                        // Не Custom лимит
                        if (commands[5] != "/0")
                            return AddLimit_DoItAndreturnCompleteMessage;
                        // Для Custom дату задаёт сам пользователь
                        else
                            return AddLimit_ChoiceLimitEndDate;
                    }
                    return DefaultAnswer;

                case 7:
                    if (commands[1] == LimitKeyPhrases.AddLimit)
                    {
                        // Получены данные о дате окончания лимита для кастомного типа
                        return AddLimit_DoItAndreturnCompleteMessage;
                    }
                    return DefaultAnswer;

                default:
                    return DefaultAnswer;
            }
        }

        #region MainActions

        private bool AddLimit(long limitId, long currencyId, double limitSumm, long LimitType,
            DateTime startPeriod, DateTime endPeriod, double creditByLastPeriod = 0.0)
        {
            try
            {
                _dbProxy.InsertLimit(new Limit()
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

        // TODO вариант на подумать

        private Func<string, List<string>, string> GetNextStep_AddLimit(string userName, List<string> commands)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
