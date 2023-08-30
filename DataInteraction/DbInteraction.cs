using DataInteraction.Helpers;
using DataInteraction.Models;
using Microsoft.Data.Sqlite;
using System.Data;

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
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = currency.ToSqlInsertCommand();

            command.ExecuteNonQuery();
            return true;
        }

        public bool InsertInto_Category(Category category)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = category.ToSqlInsertCommand();

            command.ExecuteNonQuery();
            return true;
        }

        public bool InsertInto_LimitType(LimitType limitType)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = limitType.ToSqlInsertCommand();

            command.ExecuteNonQuery();

            return true;
        }

        public bool InsertInto_Limit(Limit limit)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = limit.ToSqlInsertCommand();

            command.ExecuteNonQuery();

            return true;
        }

        public bool InsertInto_FinanceChange(FinanceChange financeChange)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = financeChange.ToSqlInsertCommand();

            command.ExecuteNonQuery();

            return true;
        }

        public bool InsertInto_Users(User user)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = user.ToSqlInsertCommand();

            command.ExecuteNonQuery();

            return true;
        }

        #endregion

        #region SELECT

        public List<Currency> GetCurrencies() {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetCurrency_Full()} FROM main.Currencies";

            List<Currency> result = new List<Currency>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Currency()
                    {
                        ID = reader.GetInt64(0),
                        Name = reader.GetString(1),
                        Sumbol = reader.GetString(2),
                        OtherNames = reader.GetString(3),
                        IsDefault = reader.GetBoolean(4),
                        IsMainStable = reader.GetBoolean(5),
                        LastExchangeRate = reader.GetDouble(6)
                    });
                }
            }
            
            return result;
        }

        public List<Category> GetCategory() {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetCategory_Full()} FROM main.Categories";

            List<Category> result = new List<Category>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Category()
                    {
                        ID = reader.GetInt64(0),
                        ParentID = reader.GetInt64(1),
                        Name = reader.GetString(2),
                        CreatedBy = reader.GetInt64(3),
                        CreateDate = reader.GetDateTime(4)
                    });
                }
            }

            return result;
        }

        public List<LimitType> GetLimitTypes() {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetLimitType_Full()} FROM main.LimitTypes";

            List<LimitType> result = new List<LimitType>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new LimitType()
                    {
                        ID = reader.GetInt64(0),
                        Name = reader.GetString(1),
                        Period = reader.GetString(2),
                        StartPeriod = reader.GetDateTime(3),
                        IsAutoProlongation = reader.GetBoolean(4),
                        IsActive = reader.GetBoolean(5)
                    });
                }
            }

            return result;
        }

        public List<Limit> GetLimits() {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetLimit_Full()} FROM main.Limits";

            List<Limit> result = new List<Limit>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Limit()
                    {
                        ID = reader.GetInt64(0),
                        StartPeriod = reader.GetDateTime(1),
                        EndPeriod = reader.GetDateTime(2),
                        LimitSumm = reader.GetDouble(3),

                    });
                }
            }

            return result;
        }

        public List<FinanceChange> GetFinanceChanges() {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetFinanceChange_Full()} FROM main.FinanceChanges";

            List<FinanceChange> result = new List<FinanceChange>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new FinanceChange()
                    {
                        ID = reader.GetInt64(0),
                        DateOfFixation = reader.GetDateTime(1),
                        Summ = reader.GetDouble(2),
                        CurrencyId = reader.GetInt64(3),
                        Comment = reader.GetString(4),
                        FixedBy = reader.GetInt64(5),
                        CategoryId = reader.GetInt64(6),
                        SumInIternationalCurrency = reader.GetDouble(7)
                    });
                }
            }

            return result;
        }

        public List<User> GetUsers() {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetUser_Full()} FROM main.Users";

            List<User> result = new List<User>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new User()
                    {
                        ID = reader.GetInt64(0),
                        TName = reader.GetString(1),
                        RealName = reader.GetString(2),
                        DateCreate = reader.GetDateTime(3)
                    });
                }
            }

            return result;
        }


        #endregion

        #region UPDATE

        #endregion

        #region DELETE
        // На данный момент нет необходимости
        #endregion
    }
}
