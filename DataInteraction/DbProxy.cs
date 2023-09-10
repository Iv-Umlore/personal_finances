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
            var res = _dChanges.GetDefaultCurrency();
            if (res == null)
                throw new Exception("Валюта по умолчанию не найдена");

            return res;
        }

        public Currency GetLikelyCurrency(string currName)
        {
            var result = _dChanges.GetLikelyCurrency(currName);
            if (result == null)
                throw new Exception("Не удалось найти похожую валюту");
            return result;   

        }

        public long GetLikelyCategoryId(string category)
        {
            var res = _dChanges.GetLikelyCategory(category);
            if (res == null)
                throw new Exception("Не удалось cопоставить категорию");

            return res.ID;
        }

        public long GetDbUserIdByUsername(string userName)
        {
            if (userName == null)
                return 0;
            return 0;
        }

        #endregion


    }
}
