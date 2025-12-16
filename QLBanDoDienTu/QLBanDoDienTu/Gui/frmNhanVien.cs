using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QLBanDoDienTu.Class;   // NhanVien + ConnectDB
using System.IO;

namespace QLBanDoDienTu.Gui
{
    public partial class frmNhanVien : Form
    {
        private NhanVien nvBUS = new NhanVien();

        public frmNhanVien()
        {
            InitializeComponent();
            AddEvents();  // Gắn sự kiện sau khi InitializeComponent()
        }

        private void AddEvents()
        {
            this.Load += frmNhanVien_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnReset.Click += btnReset_Click;
            btnXuatXML.Click += btnXuatXML_Click;
            btnNhapXML.Click += btnNhapXML_Click;
            dgvNhanVien.CellClick += dgvNhanVien_CellClick;
        }

        // =============================================================
        //                     LOAD DỮ LIỆU
        // =============================================================
        private void frmNhanVien_Load(object sender, EventArgs e)
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
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT MaNV, HoTen, ChucVu, DiaChi, SDT FROM NHANVIEN",
                        conn
                    );
                    DataTable dt = new DataTable("NhanVien");
                    da.Fill(dt);
                    dgvNhanVien.DataSource = dt;

                    dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                    dgvNhanVien.Columns["HoTen"].HeaderText = "Họ tên";
                    dgvNhanVien.Columns["ChucVu"].HeaderText = "Chức vụ";
                    dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
                    dgvNhanVien.Columns["SDT"].HeaderText = "SĐT";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Load dữ liệu: " + ex.Message);
            }
        }

        // =============================================================
        //                    RESET INPUTS
        // =============================================================
        private void ClearInputs()
        {
            txtMaNV.Text = "";
            txtHoTenNV.Text = "";
            txtChucVuNV.Text = "";
            txtDiaChiNV.Text = "";
            txtSDTNV.Text = "";

            txtMaNV.Enabled = true;
            btnThem.Enabled = true;
        }

        // =============================================================
        //                   VALIDATE
        // =============================================================
        private bool ValidateInputs()
        {
            if (txtMaNV.Text.Trim() == "")
            {
                MessageBox.Show("Nhập mã nhân viên!");
                return false;
            }
            if (txtHoTenNV.Text.Trim() == "")
            {
                MessageBox.Show("Nhập họ tên!");
                return false;
            }
            if (txtSDTNV.Text.Trim() == "")
            {
                MessageBox.Show("Nhập số điện thoại!");
                return false;
            }
            return true;
        }

        // =============================================================
        //                          THÊM
        // =============================================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                nvBUS.Them(
                    txtMaNV.Text.Trim(),
                    txtHoTenNV.Text.Trim(),
                    txtChucVuNV.Text.Trim(),
                    txtSDTNV.Text.Trim(),
                    txtDiaChiNV.Text.Trim()
                );

                MessageBox.Show("Thêm nhân viên thành công.");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        // =============================================================
        //                          SỬA
        // =============================================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                nvBUS.Sua(
                    txtMaNV.Text.Trim(),
                    txtHoTenNV.Text.Trim(),
                    txtChucVuNV.Text.Trim(),
                    txtSDTNV.Text.Trim(),
                    txtDiaChiNV.Text.Trim()
                );

                MessageBox.Show("Cập nhật nhân viên thành công.");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        // =============================================================
        //                          XÓA
        // =============================================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaNV.Text.Trim();
            if (ma == "")
            {
                MessageBox.Show("Chọn nhân viên để xóa!");
                return;
            }

            if (MessageBox.Show($"Bạn muốn xóa nhân viên {ma}?", "Xác nhận",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                nvBUS.Xoa(ma);
                MessageBox.Show("Xóa thành công.");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }

        // =============================================================
        //                   CLICK GRIDVIEW
        // =============================================================
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

            txtMaNV.Text = row.Cells["MaNV"].Value?.ToString();
            txtHoTenNV.Text = row.Cells["HoTen"].Value?.ToString();
            txtChucVuNV.Text = row.Cells["ChucVu"].Value?.ToString();
            txtDiaChiNV.Text = row.Cells["DiaChi"].Value?.ToString();
            txtSDTNV.Text = row.Cells["SDT"].Value?.ToString();

            txtMaNV.Enabled = false;
            btnThem.Enabled = false;
        }

        // =============================================================
        //                       RESET
        // =============================================================
        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearInputs();
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
                    string ma = row["MaNV"].ToString();
                    string ten = row["HoTen"].ToString();
                    string cv = row["ChucVu"].ToString();
                    string dc = row["DiaChi"].ToString();
                    string sdt = row["SDT"].ToString();

                    // Nếu nhân viên chưa tồn tại → THÊM
                    if (!nvBUS.KiemTraTonTai(ma))
                    {
                        nvBUS.Them(ma, ten, cv, sdt, dc);
                        demThem++;
                    }
                    else
                    {
                        // Nếu đã tồn tại → SỬA
                        nvBUS.Sua(ma, ten, cv, sdt, dc);
                        demSua++;
                    }
                }

                LoadData(); // Refresh lại lưới sau khi cập nhật

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

        // =============================================================
        //                    XUẤT XML (KHÔNG LỖI)
        // =============================================================
        private void btnXuatXML_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = null;

                // DataSource là DataTable
                if (dgvNhanVien.DataSource is DataTable)
                {
                    dt = ((DataTable)dgvNhanVien.DataSource).Copy();
                }
                // DataSource là BindingSource
                else if (dgvNhanVien.DataSource is BindingSource bs &&
                         bs.DataSource is DataTable)
                {
                    dt = ((DataTable)bs.DataSource).Copy();
                }
                // Tạo DataTable thủ công nếu DataSource không phải kiểu chuẩn
                else
                {
                    if (dgvNhanVien.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất.");
                        return;
                    }

                    dt = new DataTable("NhanVien");

                    // Tạo cột
                    foreach (DataGridViewColumn col in dgvNhanVien.Columns)
                    {
                        dt.Columns.Add(col.Name);
                    }

                    // Thêm dữ liệu
                    foreach (DataGridViewRow row in dgvNhanVien.Rows)
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

                // Đảm bảo có TableName
                if (string.IsNullOrWhiteSpace(dt.TableName))
                    dt.TableName = "NhanVien";

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "XML files (*.xml)|*.xml";
                    sfd.FileName = "NhanVien.xml";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        dt.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema);
                        MessageBox.Show("Xuất XML thành công.");
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
