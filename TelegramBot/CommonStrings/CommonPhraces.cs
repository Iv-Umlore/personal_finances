using System.Threading.Channels;
namespace TelegramBot.CommonStrings
{
    public static class CommonPhraces
    {
        public static string BaseMessage = $"Для управления ботом выберите одну из следующих команд:" +
            $"\r\n {KeyWords.Currency} - Работа с валютами" +
            $"\r\n {KeyWords.Category} - Работа с категориями" +
            $"\r\n {KeyWords.Limits} - Работа с денежными лимитами";

        public static string CanselCommand = $"\r\n {KeyWords.Cansel} - Отмена операции";

        public static string GetSpecSumbolErrorMessage = $"При вводе команды был(и) использован(ны) специальные символы '{KeyWords.CommonSeparator}' используемые для корректной работы программы, пожалуйста уберите эти символы из команды";

        public static string EmptyPhrase = "Была введена пустая фраза, пожалуйста введите значение";

        public static string DoneMessage = "Успешно выполнено";

        #region Methods

        public static string GetFormatMessage(string botName)
        {
            return $"Ошибка формата!\r\n\r\nОжидаю получение информации о тратах в виде: \r\n@{botName} сумма валюта(опционально) категория - Комментарий \r\nПримеры:\r\n@{botName} 1000 Еда - Купили мороженое\r\n@{botName} 20.95 лари Кафе - Сходили в Макдоналдс\r\n\r\n Соблюдение пробелов и тире обязательно!";
        }

        #endregion
    }
}
