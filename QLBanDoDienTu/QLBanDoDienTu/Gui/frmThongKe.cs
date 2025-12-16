using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using QLBanDoDienTu.Class;

namespace QLBanDoDienTu.Gui
{
    public partial class frmThongKe : Form
    {
        XuLyXML xmlHandler = new XuLyXML();

        public frmThongKe()
        {
            InitializeComponent();

            // Gán event
            this.Load += frmThongKe_Load;
            btnXuatFile.Click += btnXuatFile_Click;
            btnXemWeb.Click += btnXemWeb_Click;
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            // Điền các lựa chọn thống kê vào combobox
            comboLoai.Items.Clear();
            comboLoai.Items.Add("Tất cả");
            comboLoai.Items.Add("SANPHAM");
            comboLoai.Items.Add("KHACHHANG");
            comboLoai.Items.Add("NHANVIEN");
            comboLoai.Items.Add("HOADON");
            comboLoai.Items.Add("CHITIETHOADON");
            comboLoai.SelectedIndex = 0;
        }

        // Nút: Xuất file XML và XSLT theo loại thống kê được chọn
        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            try
            {
                string loaiThongKe = comboLoai.SelectedItem?.ToString() ?? "Tất cả";
                
                // Hỏi người dùng muốn lưu loại file nào
                DialogResult result = MessageBox.Show(
                    $"Bạn muốn lưu file nào cho thống kê '{loaiThongKe}'?\n\n" +
                    "• Yes: Lưu cả XML và XSLT\n" +
                    "• No: Chỉ lưu XML\n" +
                    "• Cancel: Hủy bỏ",
                    "Chọn loại file lưu",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                    return;

                bool xuatXML = true;
                bool xuatXSLT = (result == DialogResult.Yes);

                string xmlPath = "";
                string xsltPath = "";

                // Tạo file XML nếu cần
                if (xuatXML)
                {
                    xmlPath = xmlHandler.TaoXMLTheoLoai(loaiThongKe);
                    if (!File.Exists(xmlPath))
                    {
                        MessageBox.Show("Không tạo được file XML.");
                        return;
                    }
                }

                // Tạo file XSLT nếu cần
                if (xuatXSLT)
                {
                    xsltPath = xmlHandler.TaoXSLTTheoLoai(loaiThongKe);
                    if (!File.Exists(xsltPath))
                    {
                        MessageBox.Show("Không tạo được file XSLT.");
                        return;
                    }
                }

                // Cho người dùng chọn thư mục lưu
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Chọn thư mục để lưu file";
                    fbd.ShowNewFolderButton = true;

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        string targetFolder = fbd.SelectedPath;
                        string message = $"Lưu file thành công cho thống kê '{loaiThongKe}':\n";

                        // Copy file XML
                        if (xuatXML && !string.IsNullOrEmpty(xmlPath))
                        {
                            string targetXml = Path.Combine(targetFolder, Path.GetFileName(xmlPath));
                            File.Copy(xmlPath, targetXml, overwrite: true);
                            message += $"• XML: {targetXml}\n";
                        }

                        // Copy file XSLT
                        if (xuatXSLT && !string.IsNullOrEmpty(xsltPath))
                        {
                            string targetXslt = Path.Combine(targetFolder, Path.GetFileName(xsltPath));
                            File.Copy(xsltPath, targetXslt, overwrite: true);
                            message += $"• XSLT: {targetXslt}\n";
                        }

                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút: Xem thống kê trên web theo loại được chọn
        private void btnXemWeb_Click(object sender, EventArgs e)
        {
            try
            {
                string loaiThongKe = comboLoai.SelectedItem?.ToString() ?? "Tất cả";
                
                // Tạo file tạm thời để xem trên web (không lưu vĩnh viễn)
                string htmlPath = xmlHandler.XMLtoHTMLTheoLoai(loaiThongKe);

                if (!File.Exists(htmlPath))
                {
                    MessageBox.Show("Không tạo được file HTML. Kiểm tra dữ liệu và XSLT.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mở file HTML bằng trình duyệt mặc định
                var psi = new ProcessStartInfo
                {
                    FileName = htmlPath,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem trên web: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboLoai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
