<?xml version='1.0' encoding='UTF-8'?>
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
</xsl:stylesheet>