using DataInteraction.Models;
using System.Globalization;
using System.Text;
using TelegramBot.CommonStrings;

namespace TelegramBot.Cases.Limits
{
    // Add Limit partial
    public partial class LimitCases
    {
        private Dictionary<int, DateTime> __GeneratedStartedDate = new Dictionary<int, DateTime>(); 
        private Dictionary<int, DateTime> __GeneratedEndDate = new Dictionary<int, DateTime>();
        private Dictionary<int, Currency> __GeneratedCurrencyData = new Dictionary<int, Currency>();
        private Dictionary<int, LimitType> __GeneratedLimitTypeData = new Dictionary<int, LimitType>();

        private int datesCount = 15;

        /// <summary>
        /// Выбрать дату начала отслеживания лимита
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        private string AddLimit_ChoiceLimitStartDate(string userName, List<string> commands)
        {
            if (commands.Count != 2) 
                throw new ApplicationException("Choice limit start date - incorrect argument count");

            StringBuilder message = new StringBuilder();
            message.Append(LimitKeyPhrases.ChoiceLimitStartDate);

            __GeneratedStartedDate.Clear();
            message.Append(GeneratingDateList(__GeneratedStartedDate, datesCount));

            return message.ToString();
        }

        /// <summary>
        /// Отправить сообщение для выбора валюты
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        private string AddLimit_ChoiceCurrency(string userName, List<string> commands)
        {
            if (commands.Count != 3)
                throw new ApplicationException("Choice currency - incorrect argument count");

            // Проверка корректности введенной даты, чтобы в дальнейшем не возникло проблемы
            try
            {
                GetEnteredDate(commands, 2, __GeneratedStartedDate);
            }
            catch(ApplicationException e)
            {
                // Дата не распарсилась верно, просим перевыбрать дату.
                Console.WriteLine(e.ToString());
                RemoveLastCommand(commands);

                __GeneratedStartedDate.Clear();
                return LimitKeyPhrases.ChoiceDate_Again + 
                    GeneratingDateList(__GeneratedStartedDate, datesCount);
            }

            var currenciesList = _dbProxy.GetCurrencyList();
            __GeneratedCurrencyData.Clear();
            return LimitKeyPhrases.ChoiceCurrencyStartMessage +
                GeneratingCurrencyList(__GeneratedCurrencyData, currenciesList);
        }


        /// <summary>
        /// Выбрать размер лимита
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="commands"></param>
        /// <returns> Сообщение для пользователя </returns>
        private string AddLimit_ChoiceLimitSize(string userName, List<string> commands)
        {
            if (commands.Count != 4)
                throw new ApplicationException("Choice limit size - incorrect argument count");

            return LimitKeyPhrases.EnterLimitSize;
        }

        /// <summary>
        /// Задать тип лимита
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        private string AddLimit_ChoiceLimitType(string userName, List<string> commands)
        {
            if (commands.Count != 5)
                throw new ApplicationException("Choice limit type - incorrect argument count");

            if (!long.TryParse(commands[4], out var temp)) {

                RemoveLastCommand(commands);
                return LimitKeyPhrases.BadNumberEntered_EnterAgain;
            }

            List<LimitType> limitTypes = _dbProxy.GetLimitTypes().Where(lt => lt.IsActive).ToList();

            __GeneratedLimitTypeData.Clear();
            return LimitKeyPhrases.ChoiceLimitType + GeneratingLimitTypesList(__GeneratedLimitTypeData, limitTypes);

        }

        /// <summary>
        /// Выбрать дату окончания лимита (актуально только для кастомных лимитов)
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddLimit_ChoiceLimitEndDate(string userName, List<string> commands)
        {
            if (commands.Count != 6)
                throw new ApplicationException("Choice Limit End Date - incorrect argument count");

            StringBuilder message = new StringBuilder();
            message.Append(LimitKeyPhrases.ChoiceLimitEndDate);

            __GeneratedEndDate.Clear();
            message.Append(GeneratingDateList(__GeneratedEndDate, datesCount));

            return message.ToString();
        }

        /// <summary>
        /// Завершить процесс создания лимита
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddLimit_DoItAndreturnCompleteMessage(string userName, List<string> commands)
        {
            if (commands.Count != 6 && commands.Count != 7)
                throw new ApplicationException("Choice Limit End Date - incorrect argument count");

            if (commands.Count == 6)
            {
                DateTime startDate = GetEnteredDate(commands, 2, __GeneratedStartedDate);
                LimitType limitType = GetSelectedLimitType(commands[5]); 

                IFormatProvider provider = new CultureInfo("en-GB");
                // TODO: Check
                DateTime limitPeriod = DateTime.ParseExact(limitType.Period, "dd.MM.yyyy", provider);
                DateTime endDate = startDate.AddDays(limitPeriod.Day);
                endDate.AddMonths(limitPeriod.Month);
                endDate.AddYears(limitPeriod.Year);

                long currId = GetSelectedCurrency(commands[3]).ID;
                long limitSumm = long.Parse(commands[4]);
                long limitTypeId = limitType.ID;

                AddLimit(currId, limitSumm, limitTypeId, startDate, endDate);
            }


            if (commands.Count == 7)
            {
                // Проверка корректности введенной даты, чтобы в дальнейшем не возникло проблемы
                try
                {
                    GetEnteredDate(commands, 6, __GeneratedEndDate);
                }
                catch (ApplicationException e)
                {
                    // Дата не распарсилась верно, просим перевыбрать дату.
                    Console.WriteLine(e.ToString());
                    RemoveLastCommand(commands);

                    __GeneratedEndDate.Clear();
                    return LimitKeyPhrases.ChoiceDate_Again +
                        GeneratingDateList(__GeneratedEndDate, datesCount);
                }

                DateTime startDate = GetEnteredDate(commands, 2, __GeneratedStartedDate);
                DateTime endDate = GetEnteredDate(commands, 6, __GeneratedEndDate);

                long currId = GetSelectedCurrency(commands[3]).ID;
                long limitSumm = long.Parse(commands[4]);
                long limitTypeId = GetSelectedLimitType(commands[5]).ID;

                AddLimit(currId, limitSumm, limitTypeId, startDate, endDate);
            }

            return CommonPhraces.DoneMessage;
        }


        #region HelpMethods


        /// <summary>
        /// Получить дату начала периода
        /// </summary>
        private DateTime GetEnteredDate(List<string> commands, int position, Dictionary<int, DateTime> generatedDict)
        {
            string key = commands[position];

            DateTime result = DateTime.MinValue;

            // Если введён ключ из списка __GeneratedStartedDate
            if (key.Length < 4)
            {
                if (!int.TryParse(key.Trim('/'), out int dictKey))
                    throw new ApplicationException("Date key parse error");
                result = generatedDict[dictKey];
            }
            // Дата введена вручную необходимо распарсить
            else
            {
                IFormatProvider provider = new CultureInfo("en-GB");
                if (!DateTime.TryParseExact(key, "dd.MM.yyyy", provider, DateTimeStyles.NoCurrentDateDefault, out result))
                    throw new ApplicationException("Date parsing error");
            }

            return result;
        }

        /// <summary>
        /// Сформировать список доступных дат на <span class="int"> count </span> 
        /// </summary>
        /// <param name="dictToSave"> Сохраняет сгенерированные даты для дальнейшего выбора </param>
        /// <param name="count"> Количестов дат для генерации </param> 
        /// <returns> Список дат вида /0 - Date 1 \r\n /1 - Date 2 ... </returns>
        private string GeneratingDateList(Dictionary<int, DateTime> dictToSave, int count = 15)
        {
            StringBuilder dates = new StringBuilder();

            for (int key = 0; key < count; key++)
            {
                dictToSave[key] = DateTime.Now.AddDays(key).Date;
                dates.Append($"\r\n/{key} - {dictToSave[key].ToString("dd.MM.yy")}");
            }

            dates.Append($"\r\nИли введите дату самостоятельно в формате dd.MM.YYYY");

            return dates.ToString();
        }

        /// <summary>
        /// Сформировать список доступных дат на <span class="int"> count </span> 
        /// </summary>
        /// <param name="dictToSave"> Сохраняет сгенерированные валюты для дальнейшего выбора </param>
        /// <param name="currencies"> Список доступных для выбора валют </param> 
        /// <returns> Список дат вида /0 - Date 1 \r\n /1 - Date 2 ... </returns>
        private string GeneratingCurrencyList(Dictionary<int, Currency> dictToSave, List<Currency> currencies)
        {
            StringBuilder currensiesListMessage = new StringBuilder();

            int key = 0;
            foreach (Currency currency in currencies)
            {
                dictToSave[key] = currency;
                currensiesListMessage.Append($"\r\n /{key} - {currency.Name} ({currency.Sumbol})");
                key++;
            }

            return currensiesListMessage.ToString();
        }

        // TODO: Возможно сделать шаблонную функцию для таких случаев
        private Currency GetSelectedCurrency(string key) {
            var dictKey = int.Parse(key.Trim('/'));
            return __GeneratedCurrencyData[dictKey];
        }

        /// <summary>
        /// Сформировать список доступных дат на <span class="int"> count </span> 
        /// </summary>
        /// <param name="dictToSave"> Сохраняет сгенерированные типы для дальнейшего выбора </param>
        /// <param name="lTypes"> Список доступных для выбора типов лимита </param> 
        /// <returns> Список дат вида /0 - LimitName + Info 1 \r\n /1 - LimitName + Info 2 ... </returns>
        private string GeneratingLimitTypesList(Dictionary<int, LimitType> dictToSave, List<LimitType> lTypes)
        {
            StringBuilder currensiesListMessage = new StringBuilder();

            int key = 0;
            foreach (LimitType limitType in lTypes)
            {
                dictToSave[key] = limitType;
                currensiesListMessage.Append($"\r\n /{key} - {GetLimitTypeInformation(limitType)}");
                key++;
            }

            return currensiesListMessage.ToString();
        }

        private LimitType GetSelectedLimitType(string key)
        {
            var dictKey = int.Parse(key.Trim('/'));
            return __GeneratedLimitTypeData[dictKey];
        }

        private string GetLimitTypeInformation(LimitType lType)
        {
            StringBuilder str = new StringBuilder();
            str.Append(lType.Name + " (");
            str.Append(lType.IsAutoProlongation ? " Периодический с периодом " + lType.Period + " Дней . месяцев . лет " : " Период задаётся пользователем ");
            str.Append(" )");

            return str.ToString();
        }

        #endregion
    }
}
