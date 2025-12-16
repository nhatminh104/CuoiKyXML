<?xml version='1.0' encoding='UTF-8'?>
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
</xsl:stylesheet>