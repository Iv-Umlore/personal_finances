using System.Globalization;
using System.Text;

namespace TelegramBot.Cases.Limits
{
    // Add Limit partial
    public partial class LimitCases
    {
        private Dictionary<int, DateTime> __GeneratedStartedDate = new Dictionary<int, DateTime>();
        private int datesCount = 15;

        /// <summary>
        /// Выбрать дату начала отслеживания лимита
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddLimit_ChoiceLimitStartDate(string userName, List<string> commands)
        {
            if (commands.Count != 2) 
                throw new ApplicationException("Choice limit start date - incorrect argument count");

            StringBuilder message = new StringBuilder();
            message.Append("Выберите подходящую дату начала лимитного периода:");

            message.Append(GeneratingDateList(__GeneratedStartedDate, datesCount));

            return message.ToString();
        }

        /// <summary>
        /// Получить дату начала периода
        /// </summary>
        private DateTime GetStartDate(List<string> commands)
        {
            string key = commands[2];

            DateTime result = DateTime.MinValue;

            // Если введён ключ из списка __GeneratedStartedDate
            if (key.Length < 4)
            {
                if (!int.TryParse(key.Trim('/'), out int dictKey))
                    throw new ApplicationException("Date key parse error");
                result = __GeneratedStartedDate[dictKey];
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
        /// Отправить сообщение для выбора валюты
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddLimit_ChoiceCurrency(string userName, List<string> commands)
        {
            if (commands.Count != 3)
                throw new ApplicationException("Choice currency - incorrect argument count");

            GetStartDate(commands);

            throw new NotImplementedException();
        }


        /// <summary>
        /// Выбрать размер лимита
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="commands"></param>
        /// <returns> Сообщение для пользователя </returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddLimit_ChoiceLimitSize(string userName, List<string> commands)
        {
            if (commands.Count != 4)
                throw new ApplicationException("Choice limit size - incorrect argument count");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Задать тип лимита
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddLimit_ChoiceLimitType(string userName, List<string> commands)
        {
            if (commands.Count != 5)
                throw new ApplicationException("Choice limit type - incorrect argument count");


            throw new NotImplementedException();
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


            throw new NotImplementedException();
        }

        /// <summary>
        /// Завершить процесс создания лимита
        /// </summary>
        /// <returns> Сообщение для пользователя </returns>
        /// <exception cref="NotImplementedException"></exception>
        private string AddLimit_DoItAndreturnCompleteMessage(string userName, List<string> commands)
        {
            if (commands.Count != 6 || commands.Count != 7)
                throw new ApplicationException("Choice Limit End Date - incorrect argument count");


            throw new NotImplementedException();
        }


        #region HelpMethods

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

        #endregion
    }
}
