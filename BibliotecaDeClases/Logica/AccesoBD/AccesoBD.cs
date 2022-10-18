using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Data;
using System.Threading.Tasks;

namespace BibliotecaDeClases
{
    class AccesoBD
    {

        public static string GetConnectionString(string connectionName = "SB")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }


        public static async Task<int> Comando<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return await cnn.ExecuteAsync(sql, data);


            }

        }

        public static async Task<List<T>> LoadDataAsync<T>(string sql, [Optional] T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                var datos = await cnn.QueryAsync<T>(sql, data);
                return datos.ToList();
            }

        }

        public static async Task<T> LoadAsync<T>(string sql, [Optional] T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                var datos = await cnn.QueryAsync<T>(sql, data);
                return datos.FirstOrDefault();
            }

        }

        public static T ReturnID<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql, data).FirstOrDefault();


            }

        }

    }
    //class AccesoBDAutomotriz
    //{

    //    public static string GetConnectionString(string connectionName = "SBAuto")
    //    {
    //        return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
    //    }


    //    public static async Task<int> Comando<T>(string sql, T data)
    //    {
    //        using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
    //        {
    //            return await cnn.ExecuteAsync(sql, data);


    //        }

    //    }

    //    public static async Task<List<T>> LoadDataAsync<T>(string sql, [Optional] T data)
    //    {
    //        using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
    //        {
    //            var datos = await cnn.QueryAsync<T>(sql, data);
    //            return datos.ToList();
    //        }

    //    }

    //    public static async Task<T> LoadAsync<T>(string sql, [Optional] T data)
    //    {
    //        using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
    //        {
    //            var datos = await cnn.QueryAsync<T>(sql, data);
    //            return datos.FirstOrDefault();
    //        }

    //    }

    //    public static T ReturnID<T>(string sql, T data)
    //    {
    //        using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
    //        {
    //            return cnn.Query<T>(sql, data).FirstOrDefault();


    //        }

    //    }

    //}
    class AccesoBDAutomotriz
    {

        public static string GetConnectionString(string connectionName = "SBTaller")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }


        public static async Task<int> Comando<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return await cnn.ExecuteAsync(sql, data);


            }

        }

        public static async Task<List<T>> LoadDataAsync<T>(string sql, [Optional] T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                var datos = await cnn.QueryAsync<T>(sql, data);
                return datos.ToList();
            }

        }

        public static async Task<T> LoadAsync<T>(string sql, [Optional] T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                var datos = await cnn.QueryAsync<T>(sql, data);
                return datos.FirstOrDefault();
            }

        }

        public static T ReturnID<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql, data).FirstOrDefault();


            }

        }

    }
}
