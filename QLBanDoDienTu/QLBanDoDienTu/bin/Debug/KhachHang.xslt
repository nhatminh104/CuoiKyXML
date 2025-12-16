<?xml version='1.0' encoding='UTF-8'?>
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
</xsl:stylesheet>