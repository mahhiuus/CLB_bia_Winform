using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;

namespace Bài_Tập_Lớn.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL thongKeDAL = new ThongKeDAL();

        public double GetDoanhThuThangHienTai() => thongKeDAL.GetDoanhThuThangHienTai();

        public int GetSoHoaDonThangHienTai() => thongKeDAL.GetSoHoaDonThangHienTai();

        public int GetKhachHangMoiThangHienTai() => thongKeDAL.GetKhachHangMoiThangHienTai();

        public int GetSoBanDangHoatDong() => thongKeDAL.GetSoBanDangHoatDong();

        public double GetGiaVonThangHienTai() => thongKeDAL.GetGiaVonThangHienTai();

        // Xử lý logic tính Lợi nhuận và tạo Label ngày (dd/MM) cho Biểu đồ ngày
        public List<Dictionary<string, object>> GetBieuDoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            var ketQua = new List<Dictionary<string, object>>();
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoNgay(tuNgay, denNgay);

            foreach (var item in listRaw)
            {
                DateTime date = Convert.ToDateTime(item.NgayBan);
                double doanhThu = Convert.ToDouble(item.DoanhThu);

                // Gọi sang DAL tính giá vốn của ngày đó
                double giaVon = thongKeDAL.GetGiaVonTheoNgay(date);
                double loiNhuan = doanhThu - giaVon;

                var row = new Dictionary<string, object>
                {
                    { "ngay_ban_label", date.ToString("dd/MM") },
                    { "doanh_thu", doanhThu },
                    { "loi_nhuan", loiNhuan }
                };
                ketQua.Add(row);
            }
            return ketQua;
        }

        // Xử lý logic tính Lợi nhuận và tạo Label tháng (Tháng X) cho Biểu đồ tháng
        public List<Dictionary<string, object>> GetBieuDoTheoThang(int nam)
        {
            var ketQua = new List<Dictionary<string, object>>();
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoThang(nam);

            foreach (var item in listRaw)
            {
                int thang = Convert.ToInt32(item.Thang);
                double doanhThu = Convert.ToDouble(item.DoanhThu);

                // Gọi sang DAL tính giá vốn của tháng đó
                double giaVon = thongKeDAL.GetGiaVonTheoThang(thang, nam);
                double loiNhuan = doanhThu - giaVon;

                var row = new Dictionary<string, object>
                {
                    { "thang_label", "Tháng " + thang },
                    { "doanh_thu", doanhThu },
                    { "loi_nhuan", loiNhuan }
                };
                ketQua.Add(row);
            }
            return ketQua;
        }
    }
}