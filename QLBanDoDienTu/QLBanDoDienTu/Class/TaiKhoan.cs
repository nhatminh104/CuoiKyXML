using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class TaiKhoan
    {
        public string DangNhap(string user, string pass)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Quyen FROM TAIKHOAN WHERE TenDangNhap=@u AND MatKhau=@p", conn);

                cmd.Parameters.AddWithValue("@u", user);
                cmd.Parameters.AddWithValue("@p", pass);

                object q = cmd.ExecuteScalar();
                return q == null ? "" : q.ToString();
            }
        }
    }
}
