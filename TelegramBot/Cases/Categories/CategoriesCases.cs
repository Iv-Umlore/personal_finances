using Common.Models;
using DataInteraction;
using DataInteraction.Models;

namespace TelegramBot.Cases.Categories
{
    public partial class CategoriesCases : BaseCase
    {
        private readonly DbProxy _dbProxy;

        #region Data 

        // TODO сделать для каждого пользователя свою
        List<CategorySimpleModel> _lastCategoriesList;

        #endregion

        public CategoriesCases(DbProxy dbProxy)
        {
            _dbProxy = dbProxy;
            _lastCategoriesList = new List<CategorySimpleModel>();
        }

        protected override Func<string, List<string>, string> GetNextStep(string userName, List<string> commands)
        {
            switch (commands.Count)
            {
                case 1:
                    return GetFirstMessage;
                case 2:
                    if (commands[1] == CategoryKeyPhrases.AddCategory)
                        return AddCategory_DirectoryFirstChoice;

                    return DefaultAnswer;
                case 3:
                    if (commands[1] == CategoryKeyPhrases.AddCategory)
                        return AddCategory_DirectoryOtherChoice;

                    return DefaultAnswer;
                case 4:
                    if (commands[1] == CategoryKeyPhrases.AddCategory)
                        return AddCategory_DoItAndReturnCompleteAnswer;

                    return DefaultAnswer;
                default:
                    return DefaultAnswer;

            }
        }

        #region BaseMethods

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
        /// Отправить первое вводное сообщение с возможными вариантами взаимодействия
        /// </summary>
        private string GetFirstMessage(string userName, List<string> commands)
        {
            return CategoryKeyPhrases.ChoiceActionMessage;
        }

        #endregion
    }
}
