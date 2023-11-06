namespace TelegramBot.Cases.Limits
{
    public static class LimitKeyPhrases
    {

        #region LimitKeys

        /// <summary>
        /// Добавить лимит
        /// </summary>
        public static string AddLimitType = "/Add_LimitType";

        /// <summary>
        /// Работа с типами лимита
        /// </summary>
        public static string ChangeLimitType = "/Change_LimitType";

        /// <summary>
        /// Создать лимит
        /// - Выбор валюты, типа лимита, начало периода, долг к началу
        /// </summary>
        public static string AddLimit = "/Add_Limit";

        // Сделать лимит неактивным
        // Активировать лимит
        // Создать новый тип лимита
        //  - Повторяемый
        //  - Единовременный

        #endregion

        #region LimitPhrases

        /// <summary>
        /// Сообщение выбора действия
        /// </summary>
        public static string ChoiceActionMessage = "Выберите действие:" +
            $"\r\n{AddLimitType} - Добавить новый тип переодического лимита" +
            $"\r\n{ChangeLimitType} - Добавить новую категорию" +
            $"\r\n{AddLimit} - Добавить новую категорию";

        #endregion
    }
}
