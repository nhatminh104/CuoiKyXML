<?xml version='1.0' encoding='UTF-8'?>
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
</xsl:stylesheet>