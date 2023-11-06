namespace TelegramBot.Cases
{
    public interface IBaseCase
    {
        /// <summary>
        /// Выполнить команду
        /// </summary>
        public string ProcessTheCommand(string  userName, List<string> Commands);

        /// <summary>
        /// Отменить выполнение команды. Очистить память и тд.
        /// </summary>
        public string CancelTheCommand(string userName, List<string> Commands);
    }
}
