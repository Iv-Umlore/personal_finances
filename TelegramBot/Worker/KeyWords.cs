namespace TelegramBot.Worker
{
    public static class KeyWords
    {
        public const string BaseMessage = $"Для управления ботом выберите одну из следующих команд: \r\n {Cansel} - отмена операции";

        /// <summary>
        /// Cansel operations
        /// </summary>
        public const string Cansel = "/cansel";

        /// <summary>
        /// Operations with categories (add, update)
        /// </summary>
        public const string Category = "/category";

        /// <summary>
        /// Operations with currencies (add, update)
        /// </summary>
        public const string Currency = "/currency";

        /// <summary>
        /// Operations with limits (add, update, on/off)
        /// </summary>
        public const string Limits = "/limits";

        public const string UpdateOperation = "/update";

        public const string AddOperation = "/delete";

        public static List<string> Commands = new List<string>() { Category, Currency, Limits };

    }
}
