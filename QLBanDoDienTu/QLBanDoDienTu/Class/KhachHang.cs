using System;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class KhachHang
    {
        public bool KiemTraTonTai(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM KHACHHANG WHERE MaKH=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void Them(string ma, string ten, string sdt, string diachi)
        {
            if (KiemTraTonTai(ma))
                throw new Exception("Khách hàng đã tồn tại!");

            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO KHACHHANG VALUES(@ma,@ten,@sdt,@dc)", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@dc", diachi);

                cmd.ExecuteNonQuery();
            }
        }

        public void Sua(string ma, string ten, string sdt, string diachi)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE KHACHHANG SET HoTen=@ten, SDT=@sdt, DiaChi=@dc WHERE MaKH=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@dc", diachi);

                cmd.ExecuteNonQuery();
            }
        }

        public void Xoa(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM KHACHHANG WHERE MaKH=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
