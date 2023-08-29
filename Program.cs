using DataInteraction;
using Microsoft.Extensions.Configuration.Json;
using TelegramBot;

namespace PersonalFinances
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DbProxy proxy = new DbProxy(Configuration.GetSqlConnectionString());
            BotSheduler sheduller = new BotSheduler();

            Task<bool> startTask = sheduller.StartBot(Configuration.GetTgBotKey(), proxy);
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