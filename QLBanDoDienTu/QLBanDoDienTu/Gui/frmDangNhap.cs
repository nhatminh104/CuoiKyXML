using System;
using System.Windows.Forms;
using QLBanDoDienTu.Class;

namespace QLBanDoDienTu.Gui
{
    public partial class frmDangNhap : Form
    {
        private TaiKhoan taiKhoan;

        public frmDangNhap()
        {
            InitializeComponent();
            taiKhoan = new TaiKhoan();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            // Thiết lập focus vào textbox tên đăng nhập
            if (txtTenDangNhap != null)
                txtTenDangNhap.Focus();

            // Thiết lập password char cho textbox mật khẩu
            if (txtMatKhau != null)
                txtMatKhau.PasswordChar = '*';
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(tenDangNhap))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                // Thực hiện đăng nhập
                string quyen = taiKhoan.DangNhap(tenDangNhap, matKhau);

                if (string.IsNullOrEmpty(quyen))
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMatKhau.Clear();
                    txtTenDangNhap.Focus();
                    return;
                }

                // Đăng nhập thành công
                PhienDangNhap.DangNhap(tenDangNhap, quyen);
                
                MessageBox.Show($"Đăng nhập thành công!\nChào mừng {tenDangNhap} ({quyen})", 
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở form chính và ẩn form đăng nhập
                this.Hide();
                frmMain mainForm = new frmMain();
                mainForm.ShowDialog();
                
                // Khi form chính đóng, kiểm tra xem có đăng xuất không
                if (!PhienDangNhap.DaDangNhap)
                {
                    // Nếu đã đăng xuất, hiển thị lại form đăng nhập
                    this.Show();
                    this.ClearForm();
                }
                else
                {
                    // Nếu chưa đăng xuất (thoát ứng dụng), đóng form đăng nhập
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát ứng dụng?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép đăng nhập bằng phím Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnDangNhap_Click(sender, e);
            }
        }

        private void txtTenDangNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chuyển focus sang textbox mật khẩu khi nhấn Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtMatKhau.Focus();
            }
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Đảm bảo thoát ứng dụng khi đóng form đăng nhập
            if (!PhienDangNhap.DaDangNhap)
            {
                Application.Exit();
            }
        }

        private void ClearForm()
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtTenDangNhap.Focus();
        }
    }
}
