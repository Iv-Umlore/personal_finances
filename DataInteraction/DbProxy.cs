using Common.Models;
using DataInteraction.Models;

namespace DataInteraction
{
    public class DbProxy
    {
        private DataChanges _dChanges;

        public DbProxy(string connectionString) {
            _dChanges = new DataChanges(connectionString);

            // TODO Проверить окончание периодов лимитов
        }

        #region Insert

        public void InsertFinanceChange(FinanceChange finChange)
        {
            _dChanges.InsertFinanceChange(finChange);
        }

        public void InsertCategory(Category category)
        {
            _dChanges.InsertCategory(category);
        }

        public void InsertCurrency(Currency currency)
        {
            _dChanges.InsertCurrency(currency);
        }

        public void InsertLimit(Limit limit)
        {
            _dChanges.InsertLimit(limit);
        }

        #endregion

        #region Update

        public void UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public void UpdateCurrency(Currency currency)
        {
            throw new NotImplementedException();
        }

        public void UpdateLimit(Limit limit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Get

        public List<Currency> GetCurrencyList()
        {
            return _dChanges.GetCurrencies();
        }

        public List<Category> GetCategoryList()
        {
            return _dChanges.GetCategories();
        }

        public List<LimitType> GetLimitTypes()
        {
            return _dChanges.GetLimitTypes();
        }

        public Currency GetDefaultCurrency()
        {
            var res = _dChanges.GetDefaultCurrency();
            return res ?? throw new Exception("Валюта по умолчанию не найдена");
        }

        public Currency GetLikelyCurrency(string currName)
        {
            var result = _dChanges.GetLikelyCurrency(currName);
            return result == null ? throw new Exception("Не удалось найти похожую валюту") : result;
        }

        public long GetLikelyCategoryId(string category)
        {
            var res = _dChanges.GetLikelyCategory(category);
            return res == null ? throw new Exception("Не удалось cопоставить категорию") : res.ID;
        }

        public long GetDbUserIdByUsername(string userName)
        {
            if (userName == null)
                return 0;
            var user = _dChanges.GetUserByName(userName);
            return user == null ? throw new Exception("Пользователь не найден") : user.ID;
        }

        public List<CategorySimpleModel> GetSubdirectory(long parentId)
        {
            var categories = _dChanges.GetCategories(parentId);
            return categories.Select(it => new CategorySimpleModel()
            {
                Id = it.ID,
                Name = it.Name,
                ParentId = parentId

            }).ToList();
        }

        #endregion


    }
}
