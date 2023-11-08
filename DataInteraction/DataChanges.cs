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
            _db.OpenConnection();
            try
            {
                if (!_db.InsertInto_FinanceChange(model))
                    throw new Exception("Не удалось сохранить модель");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public void InsertCategory(Category category)
        {
            _db.OpenConnection();
            try
            {
                if (!_db.InsertInto_Category(category))
                    throw new Exception("Не удалось сохранить модель");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public void InsertCurrency(Currency currency)
        {
            _db.OpenConnection();
            try
            {
                if (_db.InsertInto_Currency(currency))
                    throw new Exception("Не удалось сохранить модель");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public void InsertLimit(Limit limit)
        {
            _db.OpenConnection();
            try
            {
                if (_db.InsertInto_Limit(limit))
                    throw new Exception("Не удалось сохранить модель");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        #endregion

        #region Get

        public Currency? GetLikelyCurrency(string curName)
        {
            _db.OpenConnection();
            Currency? res = null;

            try
            {
                var currs = _db.GetCurrencies();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }

            return res;
        }

        public Category? GetLikelyCategory(string catName)
        {
            _db.OpenConnection();
            Category? res = null;

            try
            {
                int bestCoeff = 0;

                var categories = _db.GetCategory();

                foreach (var category in categories)
                {
                    var tmp = BaseFunctions.GetLikelyIndex(catName, category.Name);
                    if (tmp > bestCoeff)
                    {
                        bestCoeff = tmp;
                        res = category;
                    }

                    if (tmp > catName.Length - 1 || tmp > category.Name.Length)
                        return res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }

            return res;
        }

        public Currency? GetDefaultCurrency()
        {
            _db.OpenConnection();
            Currency? currency = null;

            try
            {
                currency = _db.GetCurrencies().FirstOrDefault(it => it.IsDefault);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }

            return currency;
        }

        public User GetUserByName(string userName)
        {
            _db.OpenConnection();
            User? user = null;

            try
            {
                user = _db.GetUsers().FirstOrDefault(it => it.TName == $"@{userName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }

            return user;
        }

        public List<Currency> GetCurrencies()
        {
            _db.OpenConnection();
            List<Currency> currencies = new List<Currency>();

            try
            {
                currencies = _db.GetCurrencies();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }

            return currencies;
        }

        public List<LimitType> GetLimitTypes()
        {
            _db.OpenConnection();
            List<LimitType> limitTypes = new List<LimitType>();

            try
            {
                limitTypes = _db.GetLimitTypes();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }

            return limitTypes;
        }

        /// <summary>
        /// Получить категории
        /// </summary>
        /// <param name="parentId"> Id родительской папки </param>
        /// <returns></returns>
        public List<Category> GetCategories(long? parentId = null)
        {
            _db.OpenConnection();
            List<Category> categories = new List<Category>();

            try
            {
                categories = _db.GetCategory(parentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _db.CloseConnection();
            }

            return categories;
        }

        #endregion

    }
}
