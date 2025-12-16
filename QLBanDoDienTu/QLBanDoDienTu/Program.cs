using System;
using System.Windows.Forms;
using QLBanDoDienTu.Gui;

namespace QLBanDoDienTu
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Khởi chạy form đăng nhập đầu tiên
            Application.Run(new frmDangNhap());
        }
    }
}
