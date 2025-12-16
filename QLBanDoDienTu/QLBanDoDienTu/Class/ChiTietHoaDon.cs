using System;
using System.Data;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class ChiTietHoaDon
    {
        public bool KiemTraTonTai(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM CHITIETHOADON WHERE MaCTHD=@ma", conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // Lấy tất cả
        public DataTable GetAll()
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM CHITIETHOADON", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // THÊM
        public void Them(string ma, string maHD, string maSP, decimal dg, int sl, decimal thanhTien)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO CHITIETHOADON VALUES(@ma,@hd,@sp,@dg,@sl,@tt)",
                    conn
                );

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@hd", maHD);
                cmd.Parameters.AddWithValue("@sp", maSP);
                cmd.Parameters.AddWithValue("@dg", dg);
                cmd.Parameters.AddWithValue("@sl", sl);
                cmd.Parameters.AddWithValue("@tt", thanhTien);

                cmd.ExecuteNonQuery();
            }
        }

        // SỬA
        public void Sua(string ma, string maHD, string maSP, decimal dg, int sl, decimal thanhTien)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE CHITIETHOADON SET 
                        MaHD=@hd, MaSP=@sp, DonGia=@dg, SoLuong=@sl, ThanhTien=@tt
                      WHERE MaCTHD=@ma",
                    conn
                );

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@hd", maHD);
                cmd.Parameters.AddWithValue("@sp", maSP);
                cmd.Parameters.AddWithValue("@dg", dg);
                cmd.Parameters.AddWithValue("@sl", sl);
                cmd.Parameters.AddWithValue("@tt", thanhTien);

                cmd.ExecuteNonQuery();
            }
        }

        // XÓA
        public void Xoa(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM CHITIETHOADON WHERE MaCTHD=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
