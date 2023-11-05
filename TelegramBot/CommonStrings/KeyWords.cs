namespace TelegramBot.CommonStrings
{
    public static class KeyWords
    {
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



        public static string CommonSeparator = "&^&";

    }
}
