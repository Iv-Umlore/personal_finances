using DataInteraction;
using TelegramBot;

namespace PersonalFinances
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DbProxy proxy = new DbProxy(Configuration.GetSqlConnectionString());
            BotSheduler sheduller = new BotSheduler(Configuration.GetMainTelegramConfId(), proxy);

            Task<bool> startTask = sheduller.StartBot(Configuration.GetTgBotKey());
            startTask.Wait();

            if (startTask.Result)
            {
                Console.WriteLine("Бот запущен!");
            }
            else
            {
                Console.WriteLine("Возникла проблема при запуске бота");
            }
        }
    }
}