using System;
using System.Data;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class HoaDon
    {
        public DataTable GetAll()
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaHD, MaKH, MaNV, NgayLap, TongTien FROM HOADON", conn);

                DataTable dt = new DataTable("HOADON");   // TÊN TABLE RÕ RÀNG
                da.Fill(dt);

                return dt;
            }
        }

        public bool KiemTraTonTai(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM HOADON WHERE MaHD=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
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
                    @"INSERT INTO HOADON(MaHD, MaKH, MaNV, NgayLap, TongTien)
                      VALUES(@ma,@kh,@nv,@ngay,@tong)", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@kh", maKH);
                cmd.Parameters.AddWithValue("@nv", maNV);
                cmd.Parameters.AddWithValue("@ngay", ngayLap);
                cmd.Parameters.AddWithValue("@tong", tongTien);

                cmd.ExecuteNonQuery();
            }
        }

        public void Sua(string ma, string maKH, string maNV, DateTime ngayLap, decimal tongTien)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE HOADON 
                      SET MaKH=@kh, MaNV=@nv, NgayLap=@ngay, TongTien=@tong
                      WHERE MaHD=@ma", conn);

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
