using DataInteraction.Models;

namespace DataInteraction
{
    public class DbProxy
    {
        private DataChanges _dChanges;

        public DbProxy(string connectionString) {
            _dChanges = new DataChanges(connectionString);

            // Проверить окончание периодов лимитов
        }

        #region Insert

        public void InsertFinanceChange(FinanceChange model)
        {
            _dChanges.InsertFinanceChange(model);
        }

        #endregion


        #region Update

        #endregion

        #region Get

        public List<Currency> GetCurrencyList()
        {
            return _dChanges.GetCurrencies();
        }

        public List<LimitType> GetLimitTypes()
        {
            return _dChanges.GetLimitTypes();
        }

        public Currency GetDefaultCurrency()
        {
            return null;
        }

        public Currency GetLikelyCurrency(string currName)
        {

            return null;
        }

        public long GetLikelyCategoryId(string category)
        {
            return 0;
        }

        #endregion


    }
}
