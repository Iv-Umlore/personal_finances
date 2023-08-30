using DataInteraction.Models;

namespace DataInteraction
{
    public class DataChanges
    {
        private DbInteraction _db;

        public DataChanges(string connectionString)
        {
            _db = new DbInteraction(connectionString);
        }

        public List<Currency> GetCurrencies()
        {
            var res = _db.OpenConnection();
            return _db.GetCurrencies();
        }
    }
}
