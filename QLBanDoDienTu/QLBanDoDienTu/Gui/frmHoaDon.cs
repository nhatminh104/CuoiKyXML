using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLBanDoDienTu.Class;

namespace QLBanDoDienTu.Gui
{
    public partial class frmHoaDon : Form
    {
        HoaDon hdBUS = new HoaDon();

        public frmHoaDon()
        {
            InitializeComponent();

            // Sự kiện
            this.Load += frmHoaDon_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnReset.Click += btnReset_Click;
            btnXuatXML.Click += btnXuatXML_Click;
            btnNhapXML.Click += btnNhapXML_Click;
            dgvHoaDon.CellClick += dgvHoaDon_CellClick;
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            // Tắt tự động tạo cột nếu bạn tự bind
            dgvHoaDon.AutoGenerateColumns = true;

            // Màu nền chung
            dgvHoaDon.BackgroundColor = Color.FromArgb(230, 245, 255); // xanh nước nhạt
            dgvHoaDon.BorderStyle = BorderStyle.None;

            // Header
            dgvHoaDon.EnableHeadersVisualStyles = false;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215); // xanh nước đậm
            dgvHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);
            dgvHoaDon.ColumnHeadersHeight = 35;

            // Dòng dữ liệu
            dgvHoaDon.DefaultCellStyle.BackColor = Color.White;
            dgvHoaDon.DefaultCellStyle.ForeColor = Color.Black;
            dgvHoaDon.DefaultCellStyle.Font =
                new Font("Segoe UI", 9);

            // Xen kẽ màu dòng
            dgvHoaDon.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(220, 240, 250);

            // Khi chọn dòng
            dgvHoaDon.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(102, 204, 255);
            dgvHoaDon.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Canh chỉnh
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.RowHeadersVisible = false;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.MultiSelect = false;

            //Load data lên dgv
            LoadData();
        }


        private void LoadData()
        {
            dgvHoaDon.DataSource = hdBUS.GetAll();
        }

        // ========================================
        // CLICK DGV → ĐỔ LÊN TEXTBOX
        // ========================================
        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];

            txtMaHD.Text = row.Cells["MaHD"].Value.ToString();
            txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
            txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
            dtNgayLap.Value = Convert.ToDateTime(row.Cells["NgayLap"].Value);
            txtTongTien.Text = row.Cells["TongTien"].Value.ToString();
        }

        // ========================================
        // RESET
        // ========================================
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMaHD.Clear();
            txtMaKH.Clear();
            txtMaNV.Clear();
            txtTongTien.Text = "0";
            dtNgayLap.Value = DateTime.Now;
            txtMaHD.Focus();
        }

        // ========================================
        // THÊM
        // ========================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                hdBUS.Them(
                    txtMaHD.Text,
                    txtMaKH.Text,
                    txtMaNV.Text,
                    dtNgayLap.Value,
                    Convert.ToDecimal(txtTongTien.Text)
                );

                LoadData();
                MessageBox.Show("Thêm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        // ========================================
        // SỬA
        // ========================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                hdBUS.Sua(
                    txtMaHD.Text,
                    txtMaKH.Text,
                    txtMaNV.Text,
                    dtNgayLap.Value,
                    Convert.ToDecimal(txtTongTien.Text)
                );

                LoadData();
                MessageBox.Show("Sửa thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
        }

        // ========================================
        // XÓA
        // ========================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa hóa đơn này?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    hdBUS.Xoa(txtMaHD.Text);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }

        // ========================================
        // XUẤT XML (ĐÃ SỬA OK)
        // ========================================
        private void btnXuatXML_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = null;

                // Nếu DataSource là DataTable
                if (dgvHoaDon.DataSource is DataTable)
                {
                    dt = ((DataTable)dgvHoaDon.DataSource).Copy();
                }
                // Nếu DataSource là BindingSource
                else if (dgvHoaDon.DataSource is BindingSource bs &&
                         bs.DataSource is DataTable)
                {
                    dt = ((DataTable)bs.DataSource).Copy();
                }
                // Trường hợp tự tạo DataTable thủ công
                else
                {
                    if (dgvHoaDon.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất.");
                        return;
                    }

                    dt = new DataTable("HoaDon");

                    // Tạo cột
                    foreach (DataGridViewColumn col in dgvHoaDon.Columns)
                    {
                        dt.Columns.Add(col.Name);
                    }

                    // Thêm dữ liệu
                    foreach (DataGridViewRow row in dgvHoaDon.Rows)
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

                // Đảm bảo có tên bảng
                if (string.IsNullOrWhiteSpace(dt.TableName))
                    dt.TableName = "HoaDon";

                // Save file dialog
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "XML files (*.xml)|*.xml";
                    sfd.FileName = "HoaDon.xml";     // ← tự điền tên file

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        dt.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema); // ← tạo schema XSD
                        MessageBox.Show("Xuất XML thành công.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất XML: " + ex.Message);
            }
        }


        // ========================================
        // NHẬP XML
        // ========================================
        private void btnNhapXML_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML files (*.xml)|*.xml";

            if (ofd.ShowDialog() != DialogResult.OK) return;

            DataSet ds = new DataSet();
            ds.ReadXml(ofd.FileName);

            if (!ds.Tables.Contains("HOADON"))
            {
                MessageBox.Show("XML không hợp lệ!");
                return;
            }

            DataTable dt = ds.Tables["HOADON"];

            int add = 0, update = 0;

            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaHD"].ToString();
                string kh = row["MaKH"].ToString();
                string nv = row["MaNV"].ToString();
                DateTime ngay = Convert.ToDateTime(row["NgayLap"]);
                decimal tong = Convert.ToDecimal(row["TongTien"]);

                if (!hdBUS.KiemTraTonTai(ma))
                {
                    hdBUS.Them(ma, kh, nv, ngay, tong);
                    add++;
                }
                else
                {
                    hdBUS.Sua(ma, kh, nv, ngay, tong);
                    update++;
                }
            }

            LoadData();
            MessageBox.Show($"Nhập XML thành công!\nThêm: {add}\nCập nhật: {update}");
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
