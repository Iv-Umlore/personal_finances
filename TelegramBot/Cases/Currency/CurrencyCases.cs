using DataInteraction;
using DataInteraction.Models;

namespace TelegramBot.Cases
{
    public class CurrencyCases : IBaseCase
    {
        private DbProxy _dbProxy;

        public CurrencyCases(DbProxy dbProxy)
        {
            _dbProxy = dbProxy;
        }

        public string ProcessTheCommand(string fullCommand)
        {
            throw new NotImplementedException();
        }

        private bool AddCurrency(string currencyName, string otherNames, string symbol,
            double lastExchangeRate = 0.0, bool isMainStable = false, bool isDefault = false)
        {
            try
            {
                _dbProxy.InsertCurrency(new Currency() {
                    Name = currencyName,
                    OtherNames = otherNames,
                    Sumbol = symbol,
                    LastExchangeRate = lastExchangeRate,
                    IsDefault = isDefault,
                    IsMainStable = isMainStable
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private bool UpdateCurrencyExchangeRate(long currencyId, double lastExchangeRateNew)
        {
            throw new NotImplementedException();
        }

        private bool UpdateCurrency(long currId, string currencyName, string otherNames, string symbol,
            double lastExchangeRate = 0.0, bool isMainStable = false, bool isDefault = false)
        {
            try
            {
                _dbProxy.UpdateCurrency(new Currency()
                {
                    ID = currId,
                    Name = currencyName,
                    OtherNames = otherNames,
                    Sumbol = symbol,
                    LastExchangeRate = lastExchangeRate,
                    IsDefault = isDefault,
                    IsMainStable = isMainStable
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
