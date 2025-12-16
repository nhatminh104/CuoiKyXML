using System;
using System.Data;
using System.Windows.Forms;
using QLBanDoDienTu.Class;

namespace QLBanDoDienTu.Gui
{
    public partial class frmChiTietHoaDon : Form
    {
        ChiTietHoaDon cthdBUS = new ChiTietHoaDon();

        public frmChiTietHoaDon()
        {
            InitializeComponent();

            // GẮN SỰ KIỆN TÍNH THÀNH TIỀN
            txtDonGia.TextChanged += txtDonGia_TextChanged;
            txtSoLuong.TextChanged += txtSoLuong_TextChanged;

            // GẮN SỰ KIỆN NÚT
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnXuatXML.Click += btnXuatXML_Click;
            btnNhapXML.Click += btnNhapXML_Click;

            // GẮN SỰ KIỆN LOAD DATA KHI CHỌN DÒNG TRONG GRID
            dgvCTHD.CellClick += dgvCTHD_CellClick;
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvCTHD.DataSource = cthdBUS.GetAll();
        }

        // ============================================
        //      TỰ TÍNH THÀNH TIỀN
        // ============================================
        private void TinhThanhTien()
        {
            if (decimal.TryParse(txtDonGia.Text, out decimal dg) &&
                int.TryParse(txtSoLuong.Text, out int sl))
            {
                txtThanhTien.Text = (dg * sl).ToString();
            }
            else
            {
                txtThanhTien.Text = "";
            }
        }

        private void txtDonGia_TextChanged(object sender, EventArgs e) => TinhThanhTien();
        private void txtSoLuong_TextChanged(object sender, EventArgs e) => TinhThanhTien();


        // ============================================
        //      GÁN DỮ LIỆU KHI CLICK GRID
        // ============================================
        private void dgvCTHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCTHD.Rows[e.RowIndex];

                txtMaCTHD.Text = row.Cells["MaCTHD"].Value.ToString();
                txtMaHD.Text = row.Cells["MaHD"].Value.ToString();
                txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                txtThanhTien.Text = row.Cells["ThanhTien"].Value.ToString();
            }
        }

        // ============================================
        //      THÊM
        // ============================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            decimal dg = decimal.Parse(txtDonGia.Text);
            int sl = int.Parse(txtSoLuong.Text);
            decimal tt = dg * sl;

            cthdBUS.Them(
                txtMaCTHD.Text,
                txtMaHD.Text,
                txtMaSP.Text,
                dg,
                sl,
                tt
            );

            LoadData();
            MessageBox.Show("Thêm thành công!");
        }

        // ============================================
        //      SỬA
        // ============================================
        private void btnSua_Click(object sender, EventArgs e)
        {
            decimal dg = decimal.Parse(txtDonGia.Text);
            int sl = int.Parse(txtSoLuong.Text);
            decimal tt = dg * sl;

            cthdBUS.Sua(
                txtMaCTHD.Text,
                txtMaHD.Text,
                txtMaSP.Text,
                dg,
                sl,
                tt
            );

            LoadData();
            MessageBox.Show("Cập nhật thành công!");
        }

        // ============================================
        //      XÓA
        // ============================================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa chi tiết hóa đơn này?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cthdBUS.Xoa(txtMaCTHD.Text);
                LoadData();
            }
        }

        // ============================================
        //      XUẤT XML
        // ============================================
        private void btnXuatXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML files (*.xml)|*.xml";
            sfd.FileName = "ChiTietHoaDon.xml";

            if (sfd.ShowDialog() != DialogResult.OK) return;

            DataTable dt = cthdBUS.GetAll();

            if (string.IsNullOrWhiteSpace(dt.TableName))
                dt.TableName = "ChiTietHoaDon";

            dt.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema);

            MessageBox.Show("Xuất XML thành công!");
        }


        // ============================================
        //      NHẬP XML → SQL
        // ============================================
        private void btnNhapXML_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML files (*.xml)|*.xml";

            if (ofd.ShowDialog() != DialogResult.OK) return;

            DataSet ds = new DataSet();
            ds.ReadXml(ofd.FileName);

            if (ds.Tables.Count == 0)
            {
                MessageBox.Show("File XML không có dữ liệu");
                return;
            }

            DataTable dt = ds.Tables[0];
            int demThem = 0, demSua = 0;

            foreach (DataRow row in dt.Rows)
            {
                string ma = row["MaCTHD"].ToString();
                string maHD = row["MaHD"].ToString();
                string maSP = row["MaSP"].ToString();
                decimal dg = Convert.ToDecimal(row["DonGia"]);
                int sl = Convert.ToInt32(row["SoLuong"]);
                decimal tt = Convert.ToDecimal(row["ThanhTien"]);

                if (!cthdBUS.KiemTraTonTai(ma))
                {
                    cthdBUS.Them(ma, maHD, maSP, dg, sl, tt);
                    demThem++;
                }
                else
                {
                    cthdBUS.Sua(ma, maHD, maSP, dg, sl, tt);
                    demSua++;
                }
            }

            LoadData();
            MessageBox.Show($"Nhập XML hoàn tất:\nThêm: {demThem}\nCập nhật: {demSua}");
        }
    }
}
