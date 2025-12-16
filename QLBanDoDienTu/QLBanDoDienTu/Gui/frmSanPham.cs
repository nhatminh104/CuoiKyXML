using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QLBanDoDienTu.Class; // SanPham + ConnectDB

namespace QLBanDoDienTu.Gui
{
    public partial class frmSanPham : Form
    {
        private SanPham spBUS = new SanPham();

        public frmSanPham()
        {
            InitializeComponent();

            // Gắn sự kiện
            this.Load += frmSanPham_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnReset.Click += btnReset_Click;
            btnXuatXML.Click += btnXuatXML_Click;
            btnNhapXML.Click += btnNhapXML_Click;

            dgvSanPham.CellClick += dgvSanPham_CellClick;
        }

        // =========================================================
        //                   LOAD DỮ LIỆU
        // =========================================================
        private void frmSanPham_Load(object sender, EventArgs e)
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
                        "SELECT MaSP, TenSP, DonGia, SoLuong FROM SANPHAM", conn
                    );

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvSanPham.DataSource = dt;

                    dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                    dgvSanPham.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                    dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
                    dgvSanPham.Columns["SoLuong"].HeaderText = "Số lượng";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Load: " + ex.Message);
            }
        }

        // =========================================================
        //                      RESET INPUT
        // =========================================================
        private void ClearInputs()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            txtDonGia.Text = "";
            txtSoLuong.Text = "";

            txtMaSP.Enabled = true;
            btnThem.Enabled = true;
        }

        // =========================================================
        //                      VALIDATE
        // =========================================================
        private bool ValidateInputs()
        {
            if (txtMaSP.Text.Trim() == "")
            {
                MessageBox.Show("Nhập mã sản phẩm!");
                return false;
            }
            if (txtTenSP.Text.Trim() == "")
            {
                MessageBox.Show("Nhập tên sản phẩm!");
                return false;
            }
            if (!decimal.TryParse(txtDonGia.Text, out _))
            {
                MessageBox.Show("Đơn giá phải là số!");
                return false;
            }
            if (!int.TryParse(txtSoLuong.Text, out _))
            {
                MessageBox.Show("Số lượng phải là số nguyên!");
                return false;
            }
            return true;
        }

        // =========================================================
        //                          THÊM
        // =========================================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                spBUS.Them(
                    txtMaSP.Text.Trim(),
                    txtTenSP.Text.Trim(),
                    decimal.Parse(txtDonGia.Text),
                    int.Parse(txtSoLuong.Text)
                );

                MessageBox.Show("Thêm sản phẩm thành công!");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        // =========================================================
        //                          SỬA
        // =========================================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                spBUS.Sua(
                    txtMaSP.Text.Trim(),
                    txtTenSP.Text.Trim(),
                    decimal.Parse(txtDonGia.Text),
                    int.Parse(txtSoLuong.Text)
                );

                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        // =========================================================
        //                          XÓA
        // =========================================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaSP.Text.Trim();
            if (ma == "")
            {
                MessageBox.Show("Chọn sản phẩm để xóa!");
                return;
            }

            if (MessageBox.Show($"Bạn muốn xóa {ma} ?", "Xác nhận",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                spBUS.Xoa(ma);
                MessageBox.Show("Xóa thành công!");
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }

        // =========================================================
        //                      CLICK GRID
        // =========================================================
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

            txtMaSP.Text = row.Cells["MaSP"].Value?.ToString();
            txtTenSP.Text = row.Cells["TenSP"].Value?.ToString();
            txtDonGia.Text = row.Cells["DonGia"].Value?.ToString();
            txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString();

            txtMaSP.Enabled = false;
            btnThem.Enabled = false;
        }

        // =========================================================
        //                      RESET BUTTON
        // =========================================================
        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // =========================================================
        //                      XUẤT XML
        // =========================================================
        private void btnXuatXML_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (dgvSanPham.DataSource as DataTable)?.Copy();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu.");
                    return;
                }

                dt.TableName = "SanPham";

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "XML files (*.xml)|*.xml",
                    FileName = "SanPham.xml"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    dt.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema);
                    MessageBox.Show("Xuất XML thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất XML: " + ex.Message);
            }
        }

        // =========================================================
        //                NHẬP XML → SQL (THÊM / SỬA)
        // =========================================================
        private void btnNhapXML_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "XML files (*.xml)|*.xml";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                DataSet ds = new DataSet();
                ds.ReadXml(ofd.FileName);

                if (ds.Tables.Count == 0)
                {
                    MessageBox.Show("File XML không có dữ liệu.");
                    return;
                }

                DataTable dt = ds.Tables[0];

                int them = 0, sua = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string ma = row["MaSP"].ToString();
                    string ten = row["TenSP"].ToString();
                    decimal dongia = Convert.ToDecimal(row["DonGia"]);
                    int sl = Convert.ToInt32(row["SoLuong"]);

                    if (!spBUS.KiemTraTonTai(ma))
                    {
                        spBUS.Them(ma, ten, dongia, sl);
                        them++;
                    }
                    else
                    {
                        spBUS.Sua(ma, ten, dongia, sl);
                        sua++;
                    }
                }

                LoadData();
                MessageBox.Show($"Nhập XML hoàn tất.\nThêm: {them}\nCập nhật: {sua}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhập XML: " + ex.Message);
            }
        }
    }
}
