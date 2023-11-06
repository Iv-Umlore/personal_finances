using Common.Models;
using System.Text;
using TelegramBot.CommonStrings;

namespace TelegramBot.Cases.Categories
{

    // Add category partial
    public partial class CategoriesCases
    {
        /// <summary>
        /// Выбор первого подкаталога для создания новой категории
        /// </summary>
        private string AddCategory_DirectoryFirstChoice(string userName, List<string> commands)
        {
            var subDirectories = _dbProxy.GetSubdirectory(1);
            return GetCategoryCatalog(CategoryKeyPhrases.ChoiceParentCategory(CategoryKeyPhrases.DefaultCategory), 1, subDirectories);
        }

        /// <summary>
        /// Выбор следующих подкаталогов для создания новой категории
        /// </summary>
        private string AddCategory_DirectoryOtherChoice(string userName, List<string> commands)
        {
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
        }

        /// <summary>
        /// Зафиксировать полученные данные по категории в базе
        /// </summary>
        /// <returns> Сообщение об успехе </returns>
        private string AddCategory_DoItAndReturnCompleteAnswer(string userName, List<string> commands)
        {
            var userId = _dbProxy.GetDbUserIdByUsername(userName);
            AddCategory(userId, commands[3], _lastCategoriesList.Where(it => it.IdInList == 0).First().Id);
            return CommonPhraces.DoneMessage;
        }

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
                result.Append($"\r\n/{iter} - {category.Name}");
                iter++;
            }
            categories.Add(new CategorySimpleModel() { Id = parentId, IdInList = 0, Name = "Создать в этом каталоге", ParentId = parentId });

            _lastCategoriesList = categories;
            return result.ToString();
        }
    }
}
