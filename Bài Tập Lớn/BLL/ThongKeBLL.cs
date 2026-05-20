using System.Data;
using Bài_Tập_Lớn.DAL;


namespace Bài_Tập_Lớn.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL thongKeDAL = new ThongKeDAL();

        public decimal TongDoanhThu()
        {
            return thongKeDAL.TongDoanhThu();
        }

        public decimal TongTienNhap()
        {
            return thongKeDAL.TongTienNhap();
        }

        public DataTable TopSanPhamBanChay()
        {
            return thongKeDAL.TopSanPhamBanChay();
        }
    }
}