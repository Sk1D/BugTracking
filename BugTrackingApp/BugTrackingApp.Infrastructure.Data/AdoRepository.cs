using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BugTrackingApp.Infrastructure.Data
{
    public abstract class AdoRepository<T> where T : class
    {
        private readonly string _connectionString;
     //   private SqlConnection _connection => new SqlConnection(_connectionString);
        protected AdoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual T PopulateRecord(SqlDataReader reader)
        {
            return null;
        }

        protected IEnumerable<T> GetRecords(SqlCommand command)
        {
            var list = new List<T>();
            //command.Connection = _connection;
            //_connection.Open();
            //try
            //{
            //    var reader = command.ExecuteReader();
            //    try
            //    {
            //        while (reader.Read())
            //            list.Add(PopulateRecord(reader));
            //    }
            //    finally
            //    {
            //        reader.Close();
            //    }
            //}
            //finally
            //{
            //    _connection.Close();
            //}

            using (var connection = GetConnection())
            {
                command.Connection = connection;
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                        list.Add(PopulateRecord(reader));
                }
                finally
                {
                    reader.Close();
                }
            }

            return list;
        }

        protected T GetRecord(SqlCommand command)
        {
            T record = null;

            using(var connection = GetConnection())
            {
                command.Connection = connection;
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        record = PopulateRecord(reader);
                        break;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return record;
        }

        protected void ExecuteCommand(SqlCommand command)
        {
            command.CommandType = CommandType.Text;

            using (var connection = GetConnection())
            {
                command.Connection = connection;
                command.ExecuteNonQuery(); //TODO SET NOCOUNT ON added to prevent extra result sets from in stored procedure
            }
        }

        protected IEnumerable<T> ExecuteStoredProc(SqlCommand command, List<SqlParameter> parameters)
        {
            var list = new List<T>();
            command.CommandType = CommandType.StoredProcedure;

            using (var connection = GetConnection())
            {
                command.Connection = connection;
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        var record = PopulateRecord(reader);
                        if (record != null) list.Add(record);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return list;
        }

        // TODO
        protected object ExecuteScalar(SqlCommand command, List<SqlParameter> parameters)
        {
            object returnValue = null;
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                using (var connection = GetConnection())
                {
                    command.Connection = connection;

                    if (parameters != null && parameters.Count > 0)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }

        protected SqlParameter GetParameter(string parameter, object value)
        {
            var parameterObject = new SqlParameter(parameter, value ?? DBNull.Value)
            {
                Direction = ParameterDirection.Input
            };
            return parameterObject;
        }

        private SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
               
            return connection;
        }
    }
}
