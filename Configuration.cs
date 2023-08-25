using System.Configuration;

namespace PersonalFinances
{
    static public class Configuration
    {
        public static string GetTgBotKey()
        {
            var value = ConfigurationManager.AppSettings["tgBotKey"];
            if (value == null)
                throw new Exception("doesn't constain key in appsettings");

            return value;
        }
    }
}
