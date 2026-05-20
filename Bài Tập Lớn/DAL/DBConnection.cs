using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Bài_Tập_Lớn.DAL
{
    public class DBConnection
    {
        private static DBConnection instance;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static DBConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBConnection();
                }
                return instance;
            }
        }
        private DBConnection() { }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}