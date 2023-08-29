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

        public static string GetSqlConnectionString()
        {
            var value = ConfigurationManager.AppSettings["sqLiteConnection"];
            if (value == null)
                throw new Exception("doesn't constain connectionStringToSql in appsettings");

            return value;
        }
    }
}
