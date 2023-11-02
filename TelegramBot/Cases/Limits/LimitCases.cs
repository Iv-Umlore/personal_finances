using DataInteraction;
using DataInteraction.Models;

namespace TelegramBot.Cases
{
    public class LimitCases : IBaseCase
    {
        private DbProxy _dbProxy;

        public LimitCases(DbProxy dbProxy)
        {
            _dbProxy = dbProxy;
        }

        public string ProcessTheCommand(string fullCommand)
        {
            throw new NotImplementedException();
        }

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

        private bool UpdateLimit(long limitId, long currencyId, double limitSumm, long LimitType,
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
    }
}
