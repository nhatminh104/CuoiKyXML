using System;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class NhanVien
    {
        public bool KiemTraTonTai(string ma)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM NHANVIEN WHERE MaNV=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void Them(string ma, string ten, string chucvu, string sdt, string diachi)
        {
            if (KiemTraTonTai(ma))
                throw new Exception("Nhân viên đã tồn tại!");

            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO NHANVIEN VALUES(@ma,@ten,@cv,@sdt,@dc)", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@cv", chucvu);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@dc", diachi);

                cmd.ExecuteNonQuery();
            }
        }

        public void Sua(string ma, string ten, string chucvu, string sdt, string diachi)
        {
            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE NHANVIEN SET HoTen=@ten, ChucVu=@cv, SDT=@sdt, DiaChi=@dc WHERE MaNV=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@cv", chucvu);
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
                    "DELETE FROM NHANVIEN WHERE MaNV=@ma", conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
