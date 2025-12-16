using System;
using System.Data;
using System.Xml.Xsl;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text;

namespace QLBanDoDienTu.Class
{
    public class XuLyXML
    {
        // Tạo XML cho tất cả bảng (phương thức cũ)
        public void TaoXML()
        {
            string xmlPath = Path.Combine(Application.StartupPath, "DataXML.xml");
            DataSet ds = new DataSet();

            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();

                new SqlDataAdapter("SELECT * FROM SANPHAM", conn).Fill(ds, "SANPHAM");
                new SqlDataAdapter("SELECT * FROM KHACHHANG", conn).Fill(ds, "KHACHHANG");
                new SqlDataAdapter("SELECT * FROM NHANVIEN", conn).Fill(ds, "NHANVIEN");
                new SqlDataAdapter("SELECT * FROM HOADON", conn).Fill(ds, "HOADON");
                new SqlDataAdapter("SELECT * FROM CHITIETHOADON", conn).Fill(ds, "CHITIETHOADON");

                ds.WriteXml(xmlPath);
            }
        }

        // Tạo XML theo loại thống kê cụ thể
        public string TaoXMLTheoLoai(string loaiThongKe)
        {
            string fileName = "";
            DataSet ds = new DataSet();

            using (var conn = ConnectDB.GetConnection())
            {
                conn.Open();

                switch (loaiThongKe.ToUpper())
                {
                    case "SANPHAM":
                        fileName = "SanPham.xml";
                        new SqlDataAdapter("SELECT * FROM SANPHAM", conn).Fill(ds, "SANPHAM");
                        break;
                    case "KHACHHANG":
                        fileName = "KhachHang.xml";
                        new SqlDataAdapter("SELECT * FROM KHACHHANG", conn).Fill(ds, "KHACHHANG");
                        break;
                    case "NHANVIEN":
                        fileName = "NhanVien.xml";
                        new SqlDataAdapter("SELECT * FROM NHANVIEN", conn).Fill(ds, "NHANVIEN");
                        break;
                    case "HOADON":
                        fileName = "HoaDon.xml";
                        new SqlDataAdapter("SELECT * FROM HOADON", conn).Fill(ds, "HOADON");
                        break;
                    case "CHITIETHOADON":
                        fileName = "ChiTietHoaDon.xml";
                        new SqlDataAdapter("SELECT * FROM CHITIETHOADON", conn).Fill(ds, "CHITIETHOADON");
                        break;
                    default: // "Tất cả"
                        fileName = "TatCa.xml";
                        new SqlDataAdapter("SELECT * FROM SANPHAM", conn).Fill(ds, "SANPHAM");
                        new SqlDataAdapter("SELECT * FROM KHACHHANG", conn).Fill(ds, "KHACHHANG");
                        new SqlDataAdapter("SELECT * FROM NHANVIEN", conn).Fill(ds, "NHANVIEN");
                        new SqlDataAdapter("SELECT * FROM HOADON", conn).Fill(ds, "HOADON");
                        new SqlDataAdapter("SELECT * FROM CHITIETHOADON", conn).Fill(ds, "CHITIETHOADON");
                        break;
                }

                string xmlPath = Path.Combine(Application.StartupPath, fileName);
                ds.WriteXml(xmlPath);
                return xmlPath;
            }
        }

        // Tạo XSLT theo loại thống kê
        public string TaoXSLTTheoLoai(string loaiThongKe)
        {
            string fileName = "";
            string xsltContent = "";

            switch (loaiThongKe.ToUpper())
            {
                case "SANPHAM":
                    fileName = "SanPham.xslt";
                    xsltContent = TaoXSLTSanPham();
                    break;
                case "KHACHHANG":
                    fileName = "KhachHang.xslt";
                    xsltContent = TaoXSLTKhachHang();
                    break;
                case "NHANVIEN":
                    fileName = "NhanVien.xslt";
                    xsltContent = TaoXSLTNhanVien();
                    break;
                case "HOADON":
                    fileName = "HoaDon.xslt";
                    xsltContent = TaoXSLTHoaDon();
                    break;
                case "CHITIETHOADON":
                    fileName = "ChiTietHoaDon.xslt";
                    xsltContent = TaoXSLTChiTietHoaDon();
                    break;
                default: // "Tất cả"
                    fileName = "TatCa.xslt";
                    xsltContent = TaoXSLTTatCa();
                    break;
            }

            string xsltPath = Path.Combine(Application.StartupPath, fileName);
            File.WriteAllText(xsltPath, xsltContent, Encoding.UTF8);
            return xsltPath;
        }

        // Chuyển đổi XML thành HTML theo loại (chỉ tạo file tạm thời cho xem web)
        public string XMLtoHTMLTheoLoai(string loaiThongKe)
        {
            // Tạo file tạm thời cho việc xem web
            string tempXmlPath = TaoXMLTheoLoai(loaiThongKe);
            string tempXsltPath = TaoXSLTTheoLoai(loaiThongKe);
            
            string htmlFileName = loaiThongKe.ToUpper() == "TẤT CẢ" ? "TatCa.html" : $"{loaiThongKe}.html";
            string htmlPath = Path.Combine(Application.StartupPath, htmlFileName);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(tempXsltPath);
            xslt.Transform(tempXmlPath, htmlPath);

            return htmlPath;
        }

        // Tạo file XML tạm thời (chỉ để xem, không lưu vĩnh viễn)
        public string TaoXMLTamThoi(string loaiThongKe)
        {
            return TaoXMLTheoLoai(loaiThongKe);
        }

        // Tạo file XSLT tạm thời (chỉ để xem, không lưu vĩnh viễn)
        public string TaoXSLTTamThoi(string loaiThongKe)
        {
            return TaoXSLTTheoLoai(loaiThongKe);
        }

        // Phương thức cũ để tương thích
        public void XMLtoHTML()
        {
            string xmlPath = Path.Combine(Application.StartupPath, "DataXML.xml");
            string xsltPath = Path.Combine(Application.StartupPath, "DataStyle.xslt");
            string htmlPath = Path.Combine(Application.StartupPath, "HienThi.html");

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltPath);
            xslt.Transform(xmlPath, htmlPath);
        }

        public string GetHTML()
        {
            return Path.Combine(Application.StartupPath, "HienThi.html");
        }

        // Các phương thức tạo XSLT cho từng loại
        private string TaoXSLTSanPham()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
<xsl:template match='/'>
<html>
<head>
    <title>Thống kê Sản phẩm</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1 { color: #2c3e50; text-align: center; }
        table { border-collapse: collapse; width: 100%; margin-top: 20px; }
        th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }
        th { background-color: #3498db; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #e8f4fd; }
    </style>
</head>
<body>
    <h1>THỐNG KÊ SẢN PHẨM</h1>
    <table>
        <tr>
            <th>Mã SP</th>
            <th>Tên sản phẩm</th>
            <th>Đơn giá</th>
            <th>Số lượng</th>
        </tr>
        <xsl:for-each select='NewDataSet/SANPHAM'>
        <tr>
            <td><xsl:value-of select='MaSP'/></td>
            <td><xsl:value-of select='TenSP'/></td>
            <td><xsl:value-of select='format-number(DonGia, &quot;#,###&quot;)'/> VNĐ</td>
            <td><xsl:value-of select='SoLuong'/></td>
        </tr>
        </xsl:for-each>
    </table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>";
        }

        private string TaoXSLTKhachHang()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
<xsl:template match='/'>
<html>
<head>
    <title>Thống kê Khách hàng</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1 { color: #27ae60; text-align: center; }
        table { border-collapse: collapse; width: 100%; margin-top: 20px; }
        th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }
        th { background-color: #27ae60; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #e8f5e8; }
    </style>
</head>
<body>
    <h1>THỐNG KÊ KHÁCH HÀNG</h1>
    <table>
        <tr>
            <th>Mã KH</th>
            <th>Họ tên</th>
            <th>Số điện thoại</th>
            <th>Địa chỉ</th>
        </tr>
        <xsl:for-each select='NewDataSet/KHACHHANG'>
        <tr>
            <td><xsl:value-of select='MaKH'/></td>
            <td><xsl:value-of select='HoTen'/></td>
            <td><xsl:value-of select='SDT'/></td>
            <td><xsl:value-of select='DiaChi'/></td>
        </tr>
        </xsl:for-each>
    </table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>";
        }

        private string TaoXSLTNhanVien()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
<xsl:template match='/'>
<html>
<head>
    <title>Thống kê Nhân viên</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1 { color: #e74c3c; text-align: center; }
        table { border-collapse: collapse; width: 100%; margin-top: 20px; }
        th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }
        th { background-color: #e74c3c; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #fdeaea; }
    </style>
</head>
<body>
    <h1>THỐNG KÊ NHÂN VIÊN</h1>
    <table>
        <tr>
            <th>Mã NV</th>
            <th>Họ tên</th>
            <th>Chức vụ</th>
            <th>Số điện thoại</th>
            <th>Địa chỉ</th>
        </tr>
        <xsl:for-each select='NewDataSet/NHANVIEN'>
        <tr>
            <td><xsl:value-of select='MaNV'/></td>
            <td><xsl:value-of select='HoTen'/></td>
            <td><xsl:value-of select='ChucVu'/></td>
            <td><xsl:value-of select='SDT'/></td>
            <td><xsl:value-of select='DiaChi'/></td>
        </tr>
        </xsl:for-each>
    </table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>";
        }

        private string TaoXSLTHoaDon()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
<xsl:template match='/'>
<html>
<head>
    <title>Thống kê Hóa đơn</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1 { color: #f39c12; text-align: center; }
        table { border-collapse: collapse; width: 100%; margin-top: 20px; }
        th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }
        th { background-color: #f39c12; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #fef9e7; }
    </style>
</head>
<body>
    <h1>THỐNG KÊ HÓA ĐƠN</h1>
    <table>
        <tr>
            <th>Mã HĐ</th>
            <th>Mã KH</th>
            <th>Mã NV</th>
            <th>Ngày lập</th>
            <th>Tổng tiền</th>
        </tr>
        <xsl:for-each select='NewDataSet/HOADON'>
        <tr>
            <td><xsl:value-of select='MaHD'/></td>
            <td><xsl:value-of select='MaKH'/></td>
            <td><xsl:value-of select='MaNV'/></td>
            <td><xsl:value-of select='NgayLap'/></td>
            <td><xsl:value-of select='format-number(TongTien, &quot;#,###&quot;)'/> VNĐ</td>
        </tr>
        </xsl:for-each>
    </table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>";
        }

        private string TaoXSLTChiTietHoaDon()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
<xsl:template match='/'>
<html>
<head>
    <title>Thống kê Chi tiết hóa đơn</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1 { color: #9b59b6; text-align: center; }
        table { border-collapse: collapse; width: 100%; margin-top: 20px; }
        th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }
        th { background-color: #9b59b6; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #f4ecf7; }
    </style>
</head>
<body>
    <h1>THỐNG KÊ CHI TIẾT HÓA ĐƠN</h1>
    <table>
        <tr>
            <th>Mã CTHĐ</th>
            <th>Mã HĐ</th>
            <th>Mã SP</th>
            <th>Đơn giá</th>
            <th>Số lượng</th>
            <th>Giảm giá</th>
            <th>Thành tiền</th>
        </tr>
        <xsl:for-each select='NewDataSet/CHITIETHOADON'>
        <tr>
            <td><xsl:value-of select='MaCTHD'/></td>
            <td><xsl:value-of select='MaHD'/></td>
            <td><xsl:value-of select='MaSP'/></td>
            <td><xsl:value-of select='format-number(DonGia, &quot;#,###&quot;)'/> VNĐ</td>
            <td><xsl:value-of select='SoLuong'/></td>
            <td><xsl:value-of select='format-number(GiamGia, &quot;#,###&quot;)'/> VNĐ</td>
            <td><xsl:value-of select='format-number(ThanhTien, &quot;#,###&quot;)'/> VNĐ</td>
        </tr>
        </xsl:for-each>
    </table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>";
        }

        private string TaoXSLTTatCa()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
<xsl:template match='/'>
<html>
<head>
    <title>Thống kê Tất cả</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        h1, h2 { color: #34495e; text-align: center; }
        table { border-collapse: collapse; width: 100%; margin: 20px 0; }
        th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
        th { background-color: #34495e; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #ecf0f1; }
        .section { margin-bottom: 40px; }
    </style>
</head>
<body>
    <h1>THỐNG KÊ TỔNG HỢP</h1>
    
    <div class='section'>
        <h2>SẢN PHẨM</h2>
        <table>
            <tr><th>Mã SP</th><th>Tên sản phẩm</th><th>Đơn giá</th><th>Số lượng</th></tr>
            <xsl:for-each select='NewDataSet/SANPHAM'>
            <tr>
                <td><xsl:value-of select='MaSP'/></td>
                <td><xsl:value-of select='TenSP'/></td>
                <td><xsl:value-of select='format-number(DonGia, &quot;#,###&quot;)'/> VNĐ</td>
                <td><xsl:value-of select='SoLuong'/></td>
            </tr>
            </xsl:for-each>
        </table>
    </div>

    <div class='section'>
        <h2>KHÁCH HÀNG</h2>
        <table>
            <tr><th>Mã KH</th><th>Họ tên</th><th>SĐT</th><th>Địa chỉ</th></tr>
            <xsl:for-each select='NewDataSet/KHACHHANG'>
            <tr>
                <td><xsl:value-of select='MaKH'/></td>
                <td><xsl:value-of select='HoTen'/></td>
                <td><xsl:value-of select='SDT'/></td>
                <td><xsl:value-of select='DiaChi'/></td>
            </tr>
            </xsl:for-each>
        </table>
    </div>

    <div class='section'>
        <h2>NHÂN VIÊN</h2>
        <table>
            <tr><th>Mã NV</th><th>Họ tên</th><th>Chức vụ</th><th>SĐT</th><th>Địa chỉ</th></tr>
            <xsl:for-each select='NewDataSet/NHANVIEN'>
            <tr>
                <td><xsl:value-of select='MaNV'/></td>
                <td><xsl:value-of select='HoTen'/></td>
                <td><xsl:value-of select='ChucVu'/></td>
                <td><xsl:value-of select='SDT'/></td>
                <td><xsl:value-of select='DiaChi'/></td>
            </tr>
            </xsl:for-each>
        </table>
    </div>

    <div class='section'>
        <h2>HÓA ĐƠN</h2>
        <table>
            <tr><th>Mã HĐ</th><th>Mã KH</th><th>Mã NV</th><th>Ngày lập</th><th>Tổng tiền</th></tr>
            <xsl:for-each select='NewDataSet/HOADON'>
            <tr>
                <td><xsl:value-of select='MaHD'/></td>
                <td><xsl:value-of select='MaKH'/></td>
                <td><xsl:value-of select='MaNV'/></td>
                <td><xsl:value-of select='NgayLap'/></td>
                <td><xsl:value-of select='format-number(TongTien, &quot;#,###&quot;)'/> VNĐ</td>
            </tr>
            </xsl:for-each>
        </table>
    </div>

    <div class='section'>
        <h2>CHI TIẾT HÓA ĐƠN</h2>
        <table>
            <tr><th>Mã CTHĐ</th><th>Mã HĐ</th><th>Mã SP</th><th>Đơn giá</th><th>Số lượng</th><th>Giảm giá</th><th>Thành tiền</th></tr>
            <xsl:for-each select='NewDataSet/CHITIETHOADON'>
            <tr>
                <td><xsl:value-of select='MaCTHD'/></td>
                <td><xsl:value-of select='MaHD'/></td>
                <td><xsl:value-of select='MaSP'/></td>
                <td><xsl:value-of select='format-number(DonGia, &quot;#,###&quot;)'/> VNĐ</td>
                <td><xsl:value-of select='SoLuong'/></td>
                <td><xsl:value-of select='format-number(GiamGia, &quot;#,###&quot;)'/> VNĐ</td>
                <td><xsl:value-of select='format-number(ThanhTien, &quot;#,###&quot;)'/> VNĐ</td>
            </tr>
            </xsl:for-each>
        </table>
    </div>
</body>
</html>
</xsl:template>
</xsl:stylesheet>";
        }
    }
}
