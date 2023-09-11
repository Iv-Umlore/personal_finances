namespace TelegramBot.Cases
{
    public interface IBaseCase
    {
        public string ProcessTheCommand(string fullCommand);
    }
}
