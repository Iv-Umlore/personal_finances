using System.Configuration;

namespace PersonalFinances
{
    static public class Configuration
    {
        public static string GetTgBotKey()
        {
            var value = ConfigurationManager.AppSettings["tgBotKey"];
            if (value == null)
                throw new Exception("doesn't constain bot-key in appsettings");

            return value;
        }

        public static long GetMainTelegramConfId()
        {
            var value = ConfigurationManager.AppSettings["mainFinanceConfId"];
            if (!long.TryParse(value, out var result))
                throw new Exception("doesn't constain conf_Id in appsettings");

            return result;
            
        }

        public static string GetSqlConnectionString()
        {
            var value = ConfigurationManager.AppSettings["sqLiteConnection"];
            if (value == null)
                throw new Exception("doesn't constain connectionStringToSql in appsettings");

            return value;
        }
    }
}
