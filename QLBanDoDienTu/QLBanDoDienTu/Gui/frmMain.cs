using System;
using System.Windows.Forms;
using QLBanDoDienTu.Class;

namespace QLBanDoDienTu.Gui
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            // ==========================
            // GÁN SỰ KIỆN MENU
            // ==========================
            this.sảnPhẩmToolStripMenuItem.Click += menuSanPham_Click;
            this.kháchHàngToolStripMenuItem.Click += menuKhachHang_Click;
            this.nhânViênToolStripMenuItem.Click += menuNhanVien_Click;
            this.thốngKêToolStripMenuItem.Click += menuThongKe_Click;

            this.quảnLýHóaĐơnToolStripMenuItem.Click += menuQuanLyHoaDon_Click;
            this.chiTiếtHóaĐơnToolStripMenuItem.Click += menuChiTietHoaDon_Click;

            // Menu tài khoản
            this.đăngNhậpToolStripMenuItem.Click += menuDangNhap_Click;
            this.quảnLýTàiKhoảnToolStripMenuItem.Click += menuQuanLyTaiKhoan_Click;
            this.đăngXuấtToolStripMenuItem.Click += menuDangXuat_Click;

            this.Load += frmMain_Load;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (!PhienDangNhap.DaDangNhap)
            {
                MessageBox.Show("Bạn chưa đăng nhập!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Hiển thị thông tin người dùng trên title bar
            this.Text = $"Quản lý bán đồ điện tử - {PhienDangNhap.TenDangNhap} ({PhienDangNhap.Quyen})";

            // Thiết lập phân quyền menu
            ThietLapPhanQuyen();
        }

        private void ThietLapPhanQuyen()
        {
            // Mặc định ẩn tất cả menu
            sảnPhẩmToolStripMenuItem.Enabled = false;
            kháchHàngToolStripMenuItem.Enabled = false;
            nhânViênToolStripMenuItem.Enabled = false;
            thốngKêToolStripMenuItem.Enabled = false;
            quảnLýHóaĐơnToolStripMenuItem.Enabled = false;
            chiTiếtHóaĐơnToolStripMenuItem.Enabled = false;

            // Menu tài khoản luôn hiển thị
            tàiKhoảnToolStripMenuItem.Enabled = true;
            đăngNhậpToolStripMenuItem.Enabled = false; // Đã đăng nhập rồi
            đăngXuấtToolStripMenuItem.Enabled = true;

            // Phân quyền submenu quản lý tài khoản
            if (PhienDangNhap.CoQuyenQuanLyTaiKhoan())
            {
                quảnLýTàiKhoảnToolStripMenuItem.Enabled = true;
            }
            else
            {
                quảnLýTàiKhoảnToolStripMenuItem.Enabled = false;
            }

            // Phân quyền theo từng loại user
            if (PhienDangNhap.CoQuyenQuanLySanPham())
            {
                sảnPhẩmToolStripMenuItem.Enabled = true;
            }

            if (PhienDangNhap.CoQuyenQuanLyKhachHang())
            {
                kháchHàngToolStripMenuItem.Enabled = true;
            }

            if (PhienDangNhap.CoQuyenQuanLyNhanVien())
            {
                nhânViênToolStripMenuItem.Enabled = true;
            }

            if (PhienDangNhap.CoQuyenThongKe())
            {
                thốngKêToolStripMenuItem.Enabled = true;
            }

            if (PhienDangNhap.CoQuyenBanHang())
            {
                quảnLýHóaĐơnToolStripMenuItem.Enabled = true;
                chiTiếtHóaĐơnToolStripMenuItem.Enabled = true;
            }
        }

        // ==========================
        // SẢN PHẨM
        // ==========================
        private void menuSanPham_Click(object sender, EventArgs e)
        {
            frmSanPham f = new frmSanPham();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        // ==========================
        // KHÁCH HÀNG
        // ==========================
        private void menuKhachHang_Click(object sender, EventArgs e)
        {
            frmKhachHang f = new frmKhachHang();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        // ==========================
        // NHÂN VIÊN
        // ==========================
        private void menuNhanVien_Click(object sender, EventArgs e)
        {
            frmNhanVien f = new frmNhanVien();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        // ==========================
        // MENU TÀI KHOẢN
        // ==========================
        private void menuDangNhap_Click(object sender, EventArgs e)
        {
            // Không cần thiết vì đã đăng nhập rồi
            MessageBox.Show("Bạn đã đăng nhập rồi!", "Thông báo", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuQuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            frmQuanLyTaiKhoan f = new frmQuanLyTaiKhoan();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void menuDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat();
        }

        // ==========================
        // THỐNG KÊ
        // ==========================
        private void menuThongKe_Click(object sender, EventArgs e)
        {
            frmThongKe f = new frmThongKe();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        // ==========================
        // QUẢN LÝ HÓA ĐƠN
        // ==========================
        private void menuQuanLyHoaDon_Click(object sender, EventArgs e)
        {
            frmHoaDon f = new frmHoaDon();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        // ==========================
        // CHI TIẾT HÓA ĐƠN
        // ==========================
        private void menuChiTietHoaDon_Click(object sender, EventArgs e)
        {
            frmChiTietHoaDon f = new frmChiTietHoaDon();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        // ==========================
        // ĐĂNG XUẤT
        // ==========================
        private void DangXuat()
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PhienDangNhap.DangXuat();
                
                // Ẩn form chính
                this.Hide();
                
                // Hiển thị lại form đăng nhập
                frmDangNhap loginForm = new frmDangNhap();
                loginForm.ShowDialog();
                
                // Sau khi form đăng nhập đóng, đóng form chính
                this.Close();
            }
        }

        // Override form closing để xử lý đóng form
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Nếu đang đăng nhập và không phải đang đăng xuất
            if (PhienDangNhap.DaDangNhap)
            {
                // Nếu user đóng form bằng nút X, hỏi xem có muốn đăng xuất không
                if (MessageBox.Show("Bạn có muốn đăng xuất và quay lại màn hình đăng nhập?", "Xác nhận", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PhienDangNhap.DangXuat();
                    
                    // Ẩn form hiện tại
                    this.Hide();
                    
                    // Hiển thị lại form đăng nhập
                    frmDangNhap loginForm = new frmDangNhap();
                    loginForm.ShowDialog();
                }
                else
                {
                    e.Cancel = true; // Hủy việc đóng form
                    return;
                }
            }
            base.OnFormClosing(e);
        }
    }
}
