namespace TelegramBot.Cases
{
    public interface IBaseCase
    {
        /// <summary>
        /// Выполнить команду
        /// </summary>
        public string ProcessTheCommand(string  userName, List<string> Commands);
    }
}
