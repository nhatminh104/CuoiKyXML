using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QLBanDoDienTu.Class; // chứa KhachHang và ConnectDB
using System.IO;

namespace QLBanDoDienTu.Gui
{
    public partial class frmKhachHang : Form
    {
        private KhachHang khBUS = new KhachHang();

        public frmKhachHang()
        {
            InitializeComponent();
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnReset.Click += btnReset_Click;
            btnXuatXML.Click += btnXuatXML_Click;
            btnNhapXML.Click += btnNhapXML_Click;

            // ============================
            //   GÁN SỰ KIỆN CHO GRID
            // ============================
            dgvKhachHang.CellClick += dgvKhachHang_CellClick;
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
            ClearInputs();
        }

        private void LoadData()
        {
            try
            {
                using (var conn = ConnectDB.GetConnection())
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MaKH, HoTen, DiaChi, SDT FROM KHACHHANG", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvKhachHang.DataSource = dt;

                    // Tùy: sửa header
                    dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                    dgvKhachHang.Columns["HoTen"].HeaderText = "Họ tên";
                    dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
                    dgvKhachHang.Columns["SDT"].HeaderText = "SĐT";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Load dữ liệu: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtDiaChiKH.Text = "";
            txtSDTKH.Text = "";
            txtMaKH.Enabled = true; // cho phép sửa mã khi thêm
            btnThem.Enabled = true;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Nhập mã khách hàng.");
                txtMaKH.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Nhập tên khách hàng.");
                txtTenKH.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSDTKH.Text))
            {
                MessageBox.Show("Nhập số điện thoại.");
                txtSDTKH.Focus();
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                khBUS.Them(txtMaKH.Text.Trim(),
                           txtTenKH.Text.Trim(),
                           txtSDTKH.Text.Trim(),
                           txtDiaChiKH.Text.Trim());
                MessageBox.Show("Thêm thành công.");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thất bại: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                khBUS.Sua(txtMaKH.Text.Trim(),
                          txtTenKH.Text.Trim(),
                          txtSDTKH.Text.Trim(),
                          txtDiaChiKH.Text.Trim());
                MessageBox.Show("Cập nhật thành công.");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cập nhật thất bại: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaKH.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Chọn khách hàng để xóa.");
                return;
            }

            if (MessageBox.Show($"Bạn chắc muốn xóa khách hàng {ma} ?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                khBUS.Xoa(ma);
                MessageBox.Show("Xóa thành công.");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa thất bại: " + ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bảo đảm click đúng hàng
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
            txtMaKH.Text = row.Cells["MaKH"].Value?.ToString();
            txtTenKH.Text = row.Cells["HoTen"].Value?.ToString();
            txtDiaChiKH.Text = row.Cells["DiaChi"].Value?.ToString();
            txtSDTKH.Text = row.Cells["SDT"].Value?.ToString();

            txtMaKH.Enabled = false; // không cho sửa mã khi đang edit
            btnThem.Enabled = false;
        }

        // =============================================================
        //                     NHẬP XML → SQL
        // =============================================================
        private void btnNhapXML_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "XML files (*.xml)|*.xml";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                // Đọc XML
                DataSet ds = new DataSet();
                ds.ReadXml(ofd.FileName);

                if (ds.Tables.Count == 0)
                {
                    MessageBox.Show("File XML không có dữ liệu.");
                    return;
                }

                DataTable dt = ds.Tables[0];

                int demThem = 0, demSua = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string ma = row["MaKH"].ToString();
                    string ten = row["HoTen"].ToString();
                    string diachi = row["DiaChi"].ToString();
                    string sdt = row["SDT"].ToString();

                    // Nếu KH chưa tồn tại → THÊM
                    if (!khBUS.KiemTraTonTai(ma))
                    {
                        khBUS.Them(ma, ten, sdt, diachi);
                        demThem++;
                    }
                    else
                    {
                        // Nếu đã tồn tại → SỬA
                        khBUS.Sua(ma, ten, sdt, diachi);
                        demSua++;
                    }
                }

                LoadData(); // refresh grid

                MessageBox.Show(
                    $"Nhập XML hoàn tất.\nThêm mới: {demThem}\nCập nhật: {demSua}",
                    "Kết quả"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhập XML: " + ex.Message);
            }
        }


        private void btnXuatXML_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = null;

                // 1) Nếu DataSource là DataTable
                if (dgvKhachHang.DataSource is DataTable)
                {
                    dt = ((DataTable)dgvKhachHang.DataSource).Copy();
                }
                // 2) Nếu DataSource là BindingSource gắn vào DataTable
                else if (dgvKhachHang.DataSource is BindingSource bs && bs.DataSource is DataTable)
                {
                    dt = ((DataTable)bs.DataSource).Copy();
                }
                // 3) Nếu DataSource khác (ví dụ List<T>) hoặc không dùng DataSource:
                //    tạo DataTable từ DataGridView hiện tại
                else
                {
                    // Nếu không có hàng thì thoát
                    if (dgvKhachHang.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất.");
                        return;
                    }

                    dt = new DataTable("KhachHang");
                    // tạo cột từ header visible
                    foreach (DataGridViewColumn col in dgvKhachHang.Columns)
                    {
                        // lấy tên cột hợp lệ
                        string colName = string.IsNullOrEmpty(col.Name) ? col.HeaderText : col.Name;
                        if (string.IsNullOrWhiteSpace(colName)) colName = "Column" + dt.Columns.Count;
                        // nếu trùng tên, thêm hậu tố
                        string baseName = colName;
                        int idx = 1;
                        while (dt.Columns.Contains(colName))
                        {
                            colName = baseName + "_" + idx++;
                        }
                        dt.Columns.Add(colName);
                    }

                    // đổ dữ liệu từng hàng (bỏ hàng NewRow khi AllowUserToAddRows = true)
                    foreach (DataGridViewRow row in dgvKhachHang.Rows)
                    {
                        if (row.IsNewRow) continue;
                        object[] values = new object[dt.Columns.Count];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            values[i] = row.Cells[i].Value ?? DBNull.Value;
                        }
                        dt.Rows.Add(values);
                    }
                }

                // đảm bảo dt không null và có tên
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(dt.TableName))
                    dt.TableName = "KhachHang"; // bắt buộc phải có tên

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "XML files (*.xml)|*.xml";
                    sfd.FileName = "KhachHang.xml";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        dt.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema);
                        MessageBox.Show("Xuất XML thành công: " + sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất XML: " + ex.Message);
            }
        }

    }
}
