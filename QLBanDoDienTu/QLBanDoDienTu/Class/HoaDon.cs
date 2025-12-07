using System;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class HoaDon
    {
        public bool KiemTraTonTai(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM HOADON WHERE MaHD=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void Them(string ma, string maKH, string maNV, DateTime ngayLap, decimal tongTien)
        {
            if (KiemTraTonTai(ma))
                throw new Exception("Hóa đơn đã tồn tại!");

            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO HOADON VALUES(@ma,@kh,@nv,@ngay,@tong)", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@kh", maKH);
                cmd.Parameters.AddWithValue("@nv", maNV);
                cmd.Parameters.AddWithValue("@ngay", ngayLap);
                cmd.Parameters.AddWithValue("@tong", tongTien);

                cmd.ExecuteNonQuery();
            }
        }

        public void Xoa(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM HOADON WHERE MaHD=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
