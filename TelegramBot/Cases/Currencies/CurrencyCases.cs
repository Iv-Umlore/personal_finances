using DataInteraction;
using DataInteraction.Models;

namespace TelegramBot.Cases.Currencies
{
    /// <summary>
    /// Работа с валютами
    /// </summary>
    public class CurrencyCases : BaseCase
    {
        public CurrencyCases(DbProxy dbProxy) : base(dbProxy) { }

        protected override Func<string, List<string>, string> GetNextStep(string userName, List<string> commands)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавить новую валюту
        /// </summary>
        /// <param name="currencyName"></param>
        /// <param name="otherNames"></param>
        /// <param name="symbol"></param>
        /// <param name="lastExchangeRate"></param>
        /// <param name="isMainStable"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновить курс существующей валюты
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="lastExchangeRateNew"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool UpdateCurrencyExchangeRate(long currencyId, double lastExchangeRateNew)
        {
            throw new NotImplementedException();
        }

        // Обновить существующую валюту
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
