﻿using DataInteraction.Helpers;
using DataInteraction.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace DataInteraction
{
    // TODO Async
    public class DbInteraction
    {
        private SqliteConnection connection;

        public DbInteraction(string connectionString) {
            connection = new SqliteConnection(connectionString);
        }

        /// <summary>
        /// Возвращает true при успешном открытии соединения
        /// </summary>
        public bool OpenConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open &&
                    connection.State != ConnectionState.Connecting && 
                    connection.State != ConnectionState.Connecting)
                    connection.Open();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Проблема при открытии соединения: \r\n", ex.Message);
                return false; 
            }
        }

        /// <summary>
        /// Возвращает true при закрытом соединении
        /// </summary>
        public bool CloseConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    while (connection.State == ConnectionState.Connecting || connection.State == ConnectionState.Executing) ;
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Проблема при закрытии соединения: \r\n", ex.Message);
                return false;
            }
        }

        public void SaveChanges()
        {
            //connection.
        }

        #region INSERT

        public bool InsertInto_Currency(Currency currency)
        {
            SqliteCommand command = connection.CreateCommand();
            command.Connection = connection;
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

            //command.Transaction.Commit();

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

        public List<Category> GetCategory(long? parentId = null) {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetCategory_Full()} FROM main.Categories";

            if (parentId.HasValue)
            {
                command.CommandText += $" WHERE ParentID = {parentId.Value}";
            }

            List<Category> result = new List<Category>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Category()
                    {
                        ID = reader.GetInt64(0),
                        ParentID = reader.IsDBNull(1) ? -1 : reader.GetInt64(1),
                        Name = reader.GetString(2),
                        CreatedBy = reader.GetInt64(3),
                        CreateDate = Converter.StringToDate(reader.GetString(4))
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
                        StartPeriod = Converter.StringToDate(reader.GetString(3)),
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
                        StartPeriod = Converter.StringToDate(reader.GetString(1)),
                        EndPeriod = Converter.StringToDate(reader.GetString(2)),
                        LimitSumm = reader.GetDouble(3),
                        LimitTypeId = reader.GetInt64(4),
                        CreditByLastPeriod = reader.GetDouble(5),
                        CurrencyId = reader.GetInt64(6)
                    });
                }
            }

            return result;
        }

        public List<FinanceChange> GetFinanceChanges(
            DateTime? startDate = null,
            DateTime? endDate = null) {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT {SQLModelFields.GetFinanceChange_Full()} FROM main.FinanceChanges";

            if (startDate != null && endDate != null) {
                command.CommandText += " as FCh WHERE " +
                    "FCh.DateOfFixation > @startDate AND " +
                    "FCh.DateOfFixation < @endDate";
                command.Parameters.Add(new SqliteParameter("@startDate", Converter.DateToString(startDate.Value)));
                command.Parameters.Add(new SqliteParameter("@endDate", Converter.DateToString(endDate.Value)));
            }

            List<FinanceChange> result = new List<FinanceChange>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new FinanceChange()
                    {
                        ID = reader.GetInt64(0),
                        DateOfFixation = Converter.StringToDate(reader.GetString(1)),
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
                        DateCreate = Converter.StringToDate(reader.GetString(3))
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
