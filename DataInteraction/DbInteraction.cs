using DataInteraction.Models;
using Microsoft.Data.Sqlite;

namespace DataInteraction
{
    public class DbInteraction
    {
        SqliteConnection connection;

        public DbInteraction(string connectionString) {
            connection = new SqliteConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Проблема при открытии соединения: \r\n", ex.Message);
                return false; 
            }
        }

        public void SaveChanges()
        {

        }

        #region INSERT

        public bool InsertInto_Currency(Currency currency)
        {
            //SqliteCommand command = connection.CreateCommand();
            //command.CommandText = $"INSERT INTO main.'Currencies' ('Name','Sumbol','OtherNames','IsDefault','IsMainStable','LastExchangeRate') " +
            //    $"VALUES ({currency.ToSqlValues()})";

            //command.ExecuteNonQuery();
            //return true;
        }

        public bool InsertInto_Category(Category category)
        {
            return true;
        }

        public bool InsertInto_LimitType(LimitType limitType)
        {
            return true;
        }

        public bool InsertInto_Limit(Limit limit)
        {
            return true;
        }

        public bool InsertInto_FinanceChange(FinanceChange financeChange)
        {
            return true;
        }

        public bool InsertInto_Users(User user)
        {
            return true;
        }

        #endregion

        #region SELECT

        #endregion

        #region UPDATE

        #endregion

        #region DELETE
        // На данный момент нет необходимости
        #endregion
    }
}
