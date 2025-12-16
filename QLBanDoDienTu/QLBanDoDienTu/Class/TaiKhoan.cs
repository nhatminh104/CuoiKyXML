using System;
using System.Data;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class TaiKhoan
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Quyen { get; set; }

        // Đăng nhập và trả về quyền của user
        public string DangNhap(string user, string pass)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Lỗi kết nối database: " + ex.Message);
            }
        }

        // Lấy danh sách tất cả tài khoản
        public DataTable LayDanhSachTaiKhoan()
        {
            try
            {
                using (var conn = ConnectDB.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM TAIKHOAN", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách tài khoản: " + ex.Message);
            }
        }

        // Thêm tài khoản mới
        public bool ThemTaiKhoan(string tenDangNhap, string matKhau, string quyen)
        {
            try
            {
                using (var conn = ConnectDB.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO TAIKHOAN (TenDangNhap, MatKhau, Quyen) VALUES (@ten, @mk, @quyen)", conn);
                    
                    cmd.Parameters.AddWithValue("@ten", tenDangNhap);
                    cmd.Parameters.AddWithValue("@mk", matKhau);
                    cmd.Parameters.AddWithValue("@quyen", quyen);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm tài khoản: " + ex.Message);
            }
        }

        // Sửa tài khoản
        public bool SuaTaiKhoan(string tenDangNhap, string matKhau, string quyen)
        {
            try
            {
                using (var conn = ConnectDB.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE TAIKHOAN SET MatKhau=@mk, Quyen=@quyen WHERE TenDangNhap=@ten", conn);
                    
                    cmd.Parameters.AddWithValue("@ten", tenDangNhap);
                    cmd.Parameters.AddWithValue("@mk", matKhau);
                    cmd.Parameters.AddWithValue("@quyen", quyen);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi sửa tài khoản: " + ex.Message);
            }
        }

        // Xóa tài khoản
        public bool XoaTaiKhoan(string tenDangNhap)
        {
            try
            {
                using (var conn = ConnectDB.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM TAIKHOAN WHERE TenDangNhap=@ten", conn);
                    
                    cmd.Parameters.AddWithValue("@ten", tenDangNhap);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa tài khoản: " + ex.Message);
            }
        }

        // Kiểm tra tài khoản có tồn tại không
        public bool KiemTraTonTai(string tenDangNhap)
        {
            try
            {
                using (var conn = ConnectDB.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM TAIKHOAN WHERE TenDangNhap=@ten", conn);
                    
                    cmd.Parameters.AddWithValue("@ten", tenDangNhap);

                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra tài khoản: " + ex.Message);
            }
        }

        // Đổi mật khẩu
        public bool DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi)
        {
            try
            {
                using (var conn = ConnectDB.GetConnection())
                {
                    conn.Open();
                    
                    // Kiểm tra mật khẩu cũ
                    SqlCommand cmdCheck = new SqlCommand(
                        "SELECT COUNT(*) FROM TAIKHOAN WHERE TenDangNhap=@ten AND MatKhau=@mkcu", conn);
                    cmdCheck.Parameters.AddWithValue("@ten", tenDangNhap);
                    cmdCheck.Parameters.AddWithValue("@mkcu", matKhauCu);
                    
                    if ((int)cmdCheck.ExecuteScalar() == 0)
                        return false; // Mật khẩu cũ không đúng

                    // Cập nhật mật khẩu mới
                    SqlCommand cmdUpdate = new SqlCommand(
                        "UPDATE TAIKHOAN SET MatKhau=@mkmoi WHERE TenDangNhap=@ten", conn);
                    cmdUpdate.Parameters.AddWithValue("@ten", tenDangNhap);
                    cmdUpdate.Parameters.AddWithValue("@mkmoi", matKhauMoi);

                    return cmdUpdate.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đổi mật khẩu: " + ex.Message);
            }
        }
    }
}
