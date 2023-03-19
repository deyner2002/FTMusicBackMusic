using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace CORE.Loyal.DBConnection
{
    public class OracleDBConnectionSingleton
    {
        public OracleConnection oracleConnection = null;
        private static OracleDBConnectionSingleton oracleDBConnection = null;
        private OracleDBConnectionSingleton()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            oracleConnection = new OracleConnection(configuration.GetConnectionString("WebConnection"));
        }

        public static OracleDBConnectionSingleton OracleDBConnection
        {
            get
            {
                if (oracleDBConnection == null)
                    oracleDBConnection = new OracleDBConnectionSingleton();

                return oracleDBConnection;
            }
        }

        public static void OpenAsync()
        {
            if (oracleDBConnection != null)
                oracleDBConnection.oracleConnection.OpenAsync();
        }

        public static void CloseAsync()
        {
            if (oracleDBConnection != null)
                oracleDBConnection.oracleConnection.CloseAsync();
        }

    }
}
