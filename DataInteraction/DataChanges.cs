using DataInteraction.Helpers;
using DataInteraction.Models;

namespace DataInteraction
{
    public class DataChanges
    {
        private DbInteraction _db;

        public DataChanges(string connectionString)
        {
            _db = new DbInteraction(connectionString);
        }

        #region Insert

        public void InsertFinanceChange(FinanceChange model)
        {
            if (!_db.InsertInto_FinanceChange(model))
                throw new Exception("Не удалось сохранить модель");

            
        }

        #endregion

        #region Get

        public Currency GetLikelyCurrency(string curName)
        {
            _db.OpenConnection();
            var currs = _db.GetCurrencies();
            Currency res = null;
            int goodSymb = 0;

            foreach (var cur in currs)
            {
                // Полное совпадение имён
                if (cur.Name == curName)
                {
                    res = cur;
                    return res;
                }

                var otherNames = cur.OtherNames.Split(',');
                int likelyIndex = 0;
                for (int i = 0; i < otherNames.Length; i++)
                {
                    likelyIndex = BaseFunctions.GetLikelyIndex(curName, otherNames[i]); 
                    
                    if (likelyIndex > goodSymb)
                    {
                        goodSymb = likelyIndex;
                        res = cur;
                    }

                    if (likelyIndex > curName.Length - 1)
                        return res;
                    
                }
            }

            return res;
        }

        public Category GetLikelyCategory(string catName) {
            Category res = null;
            int bestCoeff = 0;

            _db.OpenConnection();
            var categories = _db.GetCategory();

            foreach (var category in categories)
            {
                var tmp = BaseFunctions.GetLikelyIndex(catName, category.Name);
                if (tmp > bestCoeff) {
                    bestCoeff = tmp;
                    res = category;
                }

                if (tmp > catName.Length - 1 || tmp > category.Name.Length)
                    return res;
            }

            return res;
        }

        public Currency GetDefaultCurrency()
        {
            return _db.GetCurrencies().FirstOrDefault(it => it.IsDefault);
        }

        public User GetUserByName(string userName)
        {
            return _db.GetUsers().FirstOrDefault(it => it.TName == $"@{userName}");
        }

        public List<Currency> GetCurrencies()
        {
            var res = _db.OpenConnection();
            return _db.GetCurrencies();
        }

        public List<LimitType> GetLimitTypes()
        {
            var res = _db.OpenConnection();
            return _db.GetLimitTypes();
        }

        #endregion

    }
}
