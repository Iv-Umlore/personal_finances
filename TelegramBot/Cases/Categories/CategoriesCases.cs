using DataInteraction;
using DataInteraction.Models;

namespace TelegramBot.Cases
{
    public class CategoriesCases : IBaseCase
    {
        private DbProxy _dbProxy;

        public CategoriesCases(DbProxy dbProxy)
        {
            _dbProxy = dbProxy;
        }

        public string ProcessTheCommand(string fullCommand)
        {
            throw new NotImplementedException();
        }

        private bool AddCategory(long userId, string categoryName, long parentCategoryId)
        {
            try
            {
                _dbProxy.InsertCategory(new Category()
                {
                    CreateDate = DateTime.Now,
                    CreatedBy = userId,
                    Name = categoryName,
                    ParentID = parentCategoryId
                });

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private bool UpdateCategory(long categoryId, long userId, string categoryName, long parentCategoryId)
        {
            try
            {
                _dbProxy.UpdateCategory(new Category()
                {
                    ID = categoryId,
                    CreateDate = DateTime.Now,
                    CreatedBy = userId,
                    Name = categoryName,
                    ParentID = parentCategoryId
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
