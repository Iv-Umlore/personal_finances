namespace TelegramBot.CommonStrings
{
    /// <summary>
    /// Общие фразы для общения с пользователем
    /// </summary>
    public static class CommonPhrases
    {
        /// <summary>
        /// Базовая инструкция для начала взаимодействия с пользователем
        /// </summary>
        public static string BaseMessage = $"Для управления ботом выберите одну из следующих команд:" +
            $"\r\n{KeyWords.Currency} - Работа с валютами" +
            $"\r\n{KeyWords.Category} - Работа с категориями" +
            $"\r\n{KeyWords.Limits} - Работа с денежными лимитами";

        /// <summary>
        /// Команда отмены
        /// </summary>
        public static string CanсelCommand = $"\r\n{KeyWords.Canсel} - Отмена операции";

        /// <summary>
        /// Введена пустая строка
        /// </summary>
        public static string EmptyPhrase = "Была введена пустая фраза, пожалуйста введите значение";

        /// <summary>
        /// Сообщение об успешном выполнении задачи
        /// </summary>
        public static string DoneMessage = "Успешно выполнено";

        #region Methods

        /// <summary>
        /// Получить сообщение об ошибке формата, для общего чата
        /// </summary>
        /// <param name="botName"> Имя бота, необходимо для формирования примера </param>
        public static string GetFormatMessage(string botName)
        {
            return $"Ошибка формата!\r\n\r\nОжидаю получение информации о тратах в виде: \r\n@{botName} сумма валюта(опционально) - Комментарий \r\nПримеры:\r\n@{botName} 1000 - Купили мороженое\r\n@{botName} 20.95 лари - Сходили в Макдоналдс\r\n\r\n Соблюдение пробелов и тире обязательно!";
        }

        public static string GetInternalErrorMessage()
        {
            return $"Возникла ошибка - Время UTC {DateTime.UtcNow}";
        }

        #endregion
    }
}
