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

        #region AddLimitPhrases

        /// <summary>
        /// Вступление перед списком дат начала лим. периода
        /// </summary>
        public static string ChoiceLimitStartDate = "Выберите подходящую дату начала лимитного периода:";

        /// <summary>
        /// Повторное вступление для выбора даты
        /// </summary>
        public static string ChoiceDate_Again = "Произошла ошибка при выборе даты, пожалуйста выберите ещё раз:";

        /// <summary>
        /// Вступление перед выбором валюты лимита
        /// </summary>
        public static string ChoiceCurrencyStartMessage = "Выберите валюту ограничения:";


        /// <summary>
        /// Сообщение Введите размер лимита
        /// </summary>
        public static string EnterLimitSize = "Пожалуйста, введите размер лимитного ограничения, для данного периода, в выбранной вами валюте";

        /// <summary>
        /// Сообщение на случай, если число невозможно распознать
        /// </summary>
        public static string BadNumberEntered_EnterAgain = "Не удалось распознать ввыведенное числовое значение, пожалуйста повторите ввод";


        public static string ChoiceLimitType = "Выберите тип лимита из списка:";

        /// <summary>
        /// Вступление перед списком дат начала лим. периода
        /// </summary>
        public static string ChoiceLimitEndDate = "Выберите подходящую дату окончания лимитного периода:";

        #endregion
    }
}
