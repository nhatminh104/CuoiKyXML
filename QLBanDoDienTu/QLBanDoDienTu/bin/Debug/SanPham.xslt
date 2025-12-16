<?xml version='1.0' encoding='UTF-8'?>
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
</xsl:stylesheet>