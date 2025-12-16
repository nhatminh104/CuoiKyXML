using System;

namespace QLBanDoDienTu.Class
{
    public static class PhienDangNhap
    {
        public static string TenDangNhap { get; set; }
        public static string Quyen { get; set; }
        public static bool DaDangNhap { get; set; }

        // Đăng nhập
        public static void DangNhap(string tenDangNhap, string quyen)
        {
            TenDangNhap = tenDangNhap;
            Quyen = quyen;
            DaDangNhap = true;
        }

        // Đăng xuất
        public static void DangXuat()
        {
            TenDangNhap = "";
            Quyen = "";
            DaDangNhap = false;
        }

        // Kiểm tra quyền
        public static bool LaQuanTri()
        {
            return DaDangNhap && Quyen == "Quản trị";
        }

        public static bool LaNhanVien()
        {
            return DaDangNhap && Quyen == "Nhân viên";
        }

        public static bool LaKeToan()
        {
            return DaDangNhap && Quyen == "Kế toán";
        }

        // Kiểm tra có quyền truy cập chức năng không
        public static bool CoQuyenQuanLyTaiKhoan()
        {
            return LaQuanTri();
        }

        public static bool CoQuyenQuanLyNhanVien()
        {
            return LaQuanTri();
        }

        public static bool CoQuyenQuanLySanPham()
        {
            return LaQuanTri() || LaNhanVien();
        }

        public static bool CoQuyenBanHang()
        {
            return LaQuanTri() || LaNhanVien();
        }

        public static bool CoQuyenThongKe()
        {
            return LaQuanTri() || LaKeToan();
        }

        public static bool CoQuyenQuanLyKhachHang()
        {
            return LaQuanTri() || LaNhanVien();
        }
    }
}