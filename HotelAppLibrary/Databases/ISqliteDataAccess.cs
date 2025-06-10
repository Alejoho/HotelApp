using System.Collections.Generic;

namespace HotelAppLibrary.Databases
{
    public interface ISqliteDataAccess
    {
        List<T> LoadData<T, U>(string sql,
                               U parameters,
                               string connectionStringName);

        void SaveData<T>(string sql,
                         T parameters,
                         string connectionStringName);
    }
}