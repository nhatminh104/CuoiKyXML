using System.Data;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLBanDoDienTu.Class
{
    public class XuLyXML
    {
        string xmlPath = Path.Combine(Application.StartupPath, "DataXML.xml");
        string xsltPath = Path.Combine(Application.StartupPath, "DataStyle.xslt");
        string htmlPath = Path.Combine(Application.StartupPath, "HienThi.html");

        public void TaoXML()
        {
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

        public void XMLtoHTML()
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltPath);
            xslt.Transform(xmlPath, htmlPath);
        }

        public string GetHTML()
        {
            return htmlPath;
        }
    }
}
