using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.GUI;

namespace Bài_Tập_Lớn
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool tiepTucChay = true;

            while (tiepTucChay)
            {
                LoginUI login = new LoginUI();
                DialogResult resultLogin = login.ShowDialog();

                if (resultLogin == DialogResult.OK)
                {
                    Maindashboard main = new Maindashboard();
                    Application.Run(main);

                    if (main.DialogResult == DialogResult.Retry)
                        tiepTucChay = true;
                    else
                        tiepTucChay = false;
                }
                else if (resultLogin == DialogResult.Retry)
                {
                    RegisterUI dangKy = new RegisterUI();
                    dangKy.ShowDialog();
                    tiepTucChay = true;
                }
                else
                {
                    // Người dùng chủ động bấm nút X ở Login để thoát hẳn app
                    tiepTucChay = false;
                }
            }
        }
    }

    public static class LichSuHeThong
    {
        public static string TenDangNhap { get; set; }
        public static string QuyenTruyCap { get; set; }
    }
}