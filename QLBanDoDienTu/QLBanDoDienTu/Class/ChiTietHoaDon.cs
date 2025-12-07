using System;
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

        public void Them(string ma, string maHD, string maSP, decimal dg, int sl, decimal? giam, decimal thanhTien)
        {
            if (KiemTraTonTai(ma))
                throw new Exception("Chi tiết hóa đơn đã tồn tại!");

            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO CHITIETHOADON VALUES(@ma,@hd,@sp,@dg,@sl,@giam,@tt)", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@hd", maHD);
                cmd.Parameters.AddWithValue("@sp", maSP);
                cmd.Parameters.AddWithValue("@dg", dg);
                cmd.Parameters.AddWithValue("@sl", sl);
                cmd.Parameters.AddWithValue("@giam", (object)giam ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tt", thanhTien);

                cmd.ExecuteNonQuery();
            }
        }

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
