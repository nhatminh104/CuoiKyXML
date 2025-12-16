using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public static class ConnectDB
    {
        public static string ConnStr =
            @"Data Source=DESKTOP-D17PSVP\SQLEXPRESS;Initial Catalog=QLBanDoDienTu;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnStr);
        }
    }
}
