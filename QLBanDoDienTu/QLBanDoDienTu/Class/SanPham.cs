using System;
using System.Data;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class SanPham
    {
        public bool KiemTraTonTai(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM SANPHAM WHERE MaSP=@ma", conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void Them(string ma, string ten, decimal dongia, int sl)
        {
            if (KiemTraTonTai(ma))
                throw new Exception("Sản phẩm đã tồn tại!");

            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO SANPHAM VALUES(@ma,@ten,@dg,@sl)", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@dg", dongia);
                cmd.Parameters.AddWithValue("@sl", sl);

                cmd.ExecuteNonQuery();
            }
        }

        public void Sua(string ma, string ten, decimal dongia, int sl)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE SANPHAM SET TenSP=@ten, DonGia=@dg, SoLuong=@sl WHERE MaSP=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@dg", dongia);
                cmd.Parameters.AddWithValue("@sl", sl);

                cmd.ExecuteNonQuery();
            }
        }

        public void Xoa(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM SANPHAM WHERE MaSP=@ma", conn);
                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
