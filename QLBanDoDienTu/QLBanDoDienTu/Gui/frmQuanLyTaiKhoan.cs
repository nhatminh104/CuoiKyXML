using System;
using System.Data;
using System.Windows.Forms;
using QLBanDoDienTu.Class;

namespace QLBanDoDienTu.Gui
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        private TaiKhoan taiKhoan;
        private bool isEditing = false;

        public frmQuanLyTaiKhoan()
        {
            InitializeComponent();
            taiKhoan = new TaiKhoan();
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền truy cập
            if (!PhienDangNhap.CoQuyenQuanLyTaiKhoan())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            LoadDanhSachTaiKhoan();
            LoadComboBoxQuyen();
            ResetForm();
        }

        private void LoadDanhSachTaiKhoan()
        {
            try
            {
                DataTable dt = taiKhoan.LayDanhSachTaiKhoan();
                dgvTaiKhoan.DataSource = dt;

                // Thiết lập tiêu đề cột
                if (dgvTaiKhoan.Columns.Count > 0)
                {
                    dgvTaiKhoan.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                    dgvTaiKhoan.Columns["MatKhau"].HeaderText = "Mật khẩu";
                    dgvTaiKhoan.Columns["Quyen"].HeaderText = "Quyền";

                    // Ẩn cột mật khẩu vì lý do bảo mật
                    dgvTaiKhoan.Columns["MatKhau"].Visible = false;
                }

                dgvTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTaiKhoan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách tài khoản: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxQuyen()
        {
            cboQuyen.Items.Clear();
            cboQuyen.Items.Add("Quản trị");
            cboQuyen.Items.Add("Nhân viên");
            cboQuyen.Items.Add("Kế toán");
            cboQuyen.SelectedIndex = 1; // Mặc định là Nhân viên
        }

        private void ResetForm()
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtXacNhanMatKhau.Clear();
            cboQuyen.SelectedIndex = 1;
            isEditing = false;
            
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            txtTenDangNhap.Enabled = true;
        }

        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];
                
                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtMatKhau.Clear(); // Không hiển thị mật khẩu
                txtXacNhanMatKhau.Clear();
                cboQuyen.Text = row.Cells["Quyen"].Value.ToString();

                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                txtTenDangNhap.Enabled = false; // Không cho sửa tên đăng nhập
                isEditing = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string quyen = cboQuyen.Text;

                // Kiểm tra tài khoản đã tồn tại
                if (taiKhoan.KiemTraTonTai(tenDangNhap))
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (taiKhoan.ThemTaiKhoan(tenDangNhap, matKhau, quyen))
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachTaiKhoan();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string quyen = cboQuyen.Text;

                if (taiKhoan.SuaTaiKhoan(tenDangNhap, matKhau, quyen))
                {
                    MessageBox.Show("Sửa tài khoản thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachTaiKhoan();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Sửa tài khoản thất bại!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = txtTenDangNhap.Text.Trim();

                // Không cho phép xóa tài khoản admin
                if (tenDangNhap.ToLower() == "admin")
                {
                    MessageBox.Show("Không thể xóa tài khoản admin!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Không cho phép xóa tài khoản đang đăng nhập
                if (tenDangNhap == PhienDangNhap.TenDangNhap)
                {
                    MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Bạn có chắc muốn xóa tài khoản '{tenDangNhap}'?", "Xác nhận", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (taiKhoan.XoaTaiKhoan(tenDangNhap))
                    {
                        MessageBox.Show("Xóa tài khoản thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachTaiKhoan();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }





        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtTenDangNhap.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtMatKhau.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (txtMatKhau.Text.Trim() != txtXacNhanMatKhau.Text.Trim())
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtXacNhanMatKhau.Focus();
                return false;
            }

            if (txtMatKhau.Text.Trim().Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            return true;
        }
    }
}
