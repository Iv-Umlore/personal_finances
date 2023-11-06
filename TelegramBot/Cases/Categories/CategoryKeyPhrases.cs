using System.Text;

namespace TelegramBot.Cases.Categories
{
    public static class CategoryKeyPhrases
    {
        public static string DefaultCategory = "Категория по умолчанию";

        #region CategoryKeys

        public static string AddCategory = "/AddCategory";

        public static string UpdateCategory = "/UpdateCategory";

        #endregion


        #region CategoryPhrase

        /// <summary>
        /// Сообщение выбора действия
        /// </summary>
        public static string ChoiceActionMessage = "Выберите действие:" +
            $"\r\n{AddCategory} - Добавить новую категорию" +
            //$"\r\n {UpdateCategory} - Обновить существующую категорию" +
            "";

        /// <summary>
        /// Запросить имя новой категории
        /// </summary>
        public static string GetCategoryNameMessage = "Введите имя категории: ";

        public static string ChoiceParentCategory(string parentName)
        {
            return $"Подкатегории для категории {parentName}";
        }

        #endregion
    }
}
