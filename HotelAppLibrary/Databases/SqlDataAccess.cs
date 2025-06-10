using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HotelAppLibrary.Databases
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        // ctor to take with DI an IConfigurable

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> LoadData<T, U>(string sql,
                                      U parameters,
                                      string connectionStringName,
                                      bool isStoreProcedure)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            CommandType commandType = isStoreProcedure is true ?
                CommandType.StoredProcedure :
                CommandType.Text;

            using IDbConnection connection = new SqlConnection(connectionString);

            List<T> data = connection.Query<T>(sql, parameters, commandType: commandType).ToList();

            return data;
        }

        public void SaveData<T>(string sql, T parameters, string connectionStringName, bool isStoreProcedure)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            CommandType commandType = isStoreProcedure is true ?
                CommandType.StoredProcedure :
                CommandType.Text;

            using IDbConnection connection = new SqlConnection(connectionString);

            connection.Execute(sql, parameters, commandType: commandType);
        }
    }
}
