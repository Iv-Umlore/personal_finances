using Common.Models;
using DataInteraction;
using DataInteraction.Models;
using System.Text;
using TelegramBot.CommonStrings;

namespace TelegramBot.Cases
{
    public class CategoriesCases : IBaseCase
    {
        private DbProxy _dbProxy;

        #region Data 

        List<CategorySimpleModel> _lastCategoriesList;

        #endregion

        public CategoriesCases(DbProxy dbProxy)
        {
            _dbProxy = dbProxy;
        }

        #region BaseMethods

        public string ProcessTheCommand(string userName, List<string> commands)
        {
            switch (commands.Count()) {
                case 1: 
                    return CategoryKeyPhrases.ChoiceActionMessage;
                case 2:
                    if (commands[1] == CategoryKeyPhrases.AddCategory)
                    {
                        var subDirectories = _dbProxy.GetSubdirectory(1);
                        return GetCategoryCatalog(CategoryKeyPhrases.ChoiceParentCategory(CategoryKeyPhrases.DefaultCategory), 1, subDirectories);
                    }
                    return "";
                case 3:
                    var catalogueInListID = long.Parse(commands[2].Trim('/'));
                    if (catalogueInListID == 0)
                        return CategoryKeyPhrases.GetCategoryNameMessage;
                    else
                    {
                        var choiceDirectory = _lastCategoriesList.First(it => it.IdInList == catalogueInListID);
                        var subDirectories = _dbProxy.GetSubdirectory(choiceDirectory.Id);
                        commands.Remove(commands.Last());
                        return GetCategoryCatalog(
                            CategoryKeyPhrases.ChoiceParentCategory(CategoryKeyPhrases.DefaultCategory),
                            choiceDirectory.Id, subDirectories);
                    }
                case 4:
                    var userId = _dbProxy.GetDbUserIdByUsername(userName);
                    AddCategory(userId, commands[3], _lastCategoriesList.Where(it => it.IdInList == 0).First().Id);
                        return CommonPhraces.DoneMessage;
                default: return "";
            }
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

        #endregion

        #region HelpMethods

        /// <summary>
        /// Получить сообщение с подкатегориями и запомнить их
        /// </summary>
        private string GetCategoryCatalog(string parentString, long parentId, List<CategorySimpleModel> categories)
        {
            var result = new StringBuilder(CategoryKeyPhrases.ChoiceParentCategory(parentString));
            result.Append("\r\n/0 - Создать в этом каталоге");

            int iter = 1;
            foreach (var category in categories)
            {
                category.IdInList = iter;
                result.Append($"\r\n/{iter} - {category}");
                iter++;
            }
            categories.Add(new CategorySimpleModel() { Id = parentId, IdInList = 0, Name = "Создать в этом каталоге", ParentId = parentId });

            _lastCategoriesList = categories;
            return result.ToString();
        }

        #endregion
    }
}
