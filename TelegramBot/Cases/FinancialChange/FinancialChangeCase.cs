using DataInteraction;
using DataInteraction.Models;
using System.Text;
using System.Xml.Linq;
using TelegramBot.CommonStrings;

namespace TelegramBot.Cases.FinancialChange
{
    public class FinancialChangeCase : BaseCase
    {
        #region Data

        /// <summary>
        /// Время последнего обработанного сообщения
        /// </summary>
        private DateTime _lastMessageDate;

        /// <summary>
        /// Последнее время обновления сообщения
        /// </summary>
        private DateTime _lastUpdate;

        /// <summary>
        /// BotName
        /// </summary>
        private string _botName;

        private Dictionary<int, Category> __CategoriesAdded;

        private string NextSubcategoriesSumbol = "> ";

        private string dictKeySeparator = "_";

        #endregion

        public FinancialChangeCase(DbProxy dbProxy) : base(dbProxy) {
            _lastUpdate = DateTime.MinValue;
            _lastMessageDate = DateTime.MinValue;
            __CategoriesAdded = new Dictionary<int, Category>();
        }

        protected override Func<string, List<string>, string> GetNextStep(string userName, List<string> commands)
        {
            if (_lastUpdate < DateTime.Now.AddMinutes(-10))
            {
                commands.Clear();
                throw new TimeoutException("Последнее сообщение было отправлено более 10 минут назад");
            }

            switch (commands.Count)
            {
                case 1:
                    return OldLogic;
                case 2:
                    return FinalStep;
                default:
                    return DefaultAnswer;
            }
        }

        public void SetHelpfullInfo(DateTime messageDate, string botName)
        {
            _botName = botName;
            _lastMessageDate = messageDate;
            _lastUpdate = DateTime.Now;
        }

        private string OldLogic(string userName, List<string> commands)
        {
            // Хочу получать сообщения вида @bot сумма валюта - комментарий
            string[] spliting = commands[0].Split('-');

            if (spliting.Length != 2)
                return CommonPhrases.GetFormatMessage(_botName);

            string comment = spliting[1];
            string[] mainInfos = spliting[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (mainInfos.Length != 2 && mainInfos.Length != 3)
                return CommonPhrases.GetFormatMessage(_botName);

            List<Category> categories = _dbProxy.GetCategoryList();
            __CategoriesAdded.Clear();
            return CreateChoiceCategoryMessage(__CategoriesAdded, categories);
        }

        private string FinalStep(string userName, List<string> commands)
        {
            string key = commands[1].TrimStart('/').Split(dictKeySeparator)[0];
            if (!int.TryParse(key, out int dictKey))
                throw new ApplicationException("Can't parse category key");

            // Хочу получать сообщения вида @bot сумма валюта - комментарий
            string[] spliting = commands[0].Split('-');

            if (spliting.Length != 2)
                return CommonPhrases.GetFormatMessage(_botName);

            string comment = spliting[1];
            string[] mainInfos = spliting[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (mainInfos.Length != 2 && mainInfos.Length != 3)
                return CommonPhrases.GetFormatMessage(_botName);

            long categoryId = __CategoriesAdded[dictKey].ID;
            var node = GetFinanceChangeForInsert(mainInfos, comment, categoryId);
            node.DateOfFixation = _lastMessageDate;
            node.FixedBy = _dbProxy.GetDbUserIdByUsername(userName);

            _dbProxy.InsertFinanceChange(node);
            __CategoriesAdded.Clear();

            commands.Clear();
            return CommonPhrases.DoneMessage;
        }

        /// <summary>
        /// Создать модель для отправки в БД
        /// </summary>
        private FinanceChange GetFinanceChangeForInsert(string[] messagePart, string comment, long categoryId)
        {
            // Нужно понять что за валюта
            if (messagePart.Length == 4)
            {
                Currency cur = _dbProxy.GetLikelyCurrency(messagePart[2]);
                var sum = double.Parse(messagePart[1]);
                FinanceChange financeChange = new FinanceChange()
                {
                    Summ = sum,
                    CurrencyId = cur.ID,
                    CategoryId = categoryId,
                    Comment = comment,
                    SumInIternationalCurrency = sum / cur.LastExchangeRate
                };
                return financeChange;
            }

            // Валюта по умолчанию
            if (messagePart.Length == 3)
            {
                Currency cur = _dbProxy.GetDefaultCurrency();
                var sum = double.Parse(messagePart[1]);
                FinanceChange financeChange = new FinanceChange()
                {
                    Summ = sum,
                    CurrencyId = cur.ID,
                    CategoryId = categoryId,
                    Comment = comment,
                    SumInIternationalCurrency = sum / cur.LastExchangeRate
                };
                return financeChange;
            }

            throw new Exception("Проблема с получением информации о категории");
        }

        private string CreateChoiceCategoryMessage(Dictionary<int, Category> dictForSave, List<Category> categories)
        {
            // TODO: Nullable type
            // Родительская папка (фактически переделать бы на NULL)
            Category root = categories.First(it => it.ParentID == -1);
            if (root == null)
                throw new ApplicationException("Havn't root category !!! ");

            StringBuilder result = new StringBuilder();
            int keyCounter = 0;

            RecursiveAddinCategories(categories, dictForSave, result, root, 0, ref keyCounter);

            return result.ToString();
        }

        /// <summary>
        /// Рекурсивное формирование списка
        /// </summary>
        /// <param name="categories"> Список всех категорий (ссылочное) </param>
        /// <param name="dictForSave"> Словарь для сохранения вариантов </param>
        /// <param name="builder"> Для формирования итоговой строки </param>
        /// <param name="heigthCall"> Глубина погружения </param>
        private void RecursiveAddinCategories(
            List<Category> categories,
            Dictionary<int, Category> dictForSave, 
            StringBuilder builder,
            Category root,
            int heigthCall,
            ref int keyCounter) 
        {
            // TODO: Ограничение не должно быть на этом уровне. Должно быть на уровне создания
            if (heigthCall > 6)
            {
                Console.WriteLine("Слишком много вложенных категорий");
                return;
            }

            builder.Append("\r\n");
            for (int i = 0; i < heigthCall; i++)
                builder.Append(NextSubcategoriesSumbol);
            builder.Append($"/{keyCounter}{dictKeySeparator}{_botName} - {root.Name}");
            dictForSave[keyCounter] = root;
            keyCounter++;

            var subCategories = categories.Where(cat => cat.ParentID == root.ID).ToList();
            foreach(Category category in subCategories)
                RecursiveAddinCategories(categories, dictForSave, builder, category, heigthCall + 1, ref keyCounter);
        }
    }
}
