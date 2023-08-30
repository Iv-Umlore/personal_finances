namespace DataInteraction
{
    public class DbProxy
    {
        private DataChanges _dChanges;

        public DbProxy(string connectionString) {
            _dChanges = new DataChanges(connectionString);

            // Проверить окончание периодов лимитов
        }
    }
}
