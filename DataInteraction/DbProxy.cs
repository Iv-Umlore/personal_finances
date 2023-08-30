using DataInteraction.Models;

namespace DataInteraction
{
    public class DbProxy
    {
        private DataChanges _dChanges;

        public DbProxy(string connectionString) {
            _dChanges = new DataChanges(connectionString);

            // Проверить окончание периодов лимитов
        }

        public List<Currency> GetCurrencyList()
        {
            return _dChanges.GetCurrencies();
        }

        public List<LimitType> GetLimitTypes()
        {
            return _dChanges.GetLimitTypes();
        }
    }
}
