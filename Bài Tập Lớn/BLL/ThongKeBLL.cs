using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;

namespace Bài_Tập_Lớn.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL thongKeDAL = new ThongKeDAL();

        // ══════════════════════════════════════════════════════════
        //  CARDS – pass thẳng xuống DAL
        // ══════════════════════════════════════════════════════════
        public double GetDoanhThuThangHienTai() => thongKeDAL.GetDoanhThuThangHienTai();
        public int GetSoHoaDonThangHienTai() => thongKeDAL.GetSoHoaDonThangHienTai();
        public int GetKhachHangMoiThangHienTai() => thongKeDAL.GetKhachHangMoiThangHienTai();
        public int GetSoBanDangHoatDong() => thongKeDAL.GetSoBanDangHoatDong();
        public double GetGiaVonThangHienTai() => thongKeDAL.GetGiaVonThangHienTai();

        // ══════════════════════════════════════════════════════════
        //  [MỚI] Tổng TienBida + TienSanPham tháng hiện tại
        //  → Dùng cho Pie Chart (tỉ lệ thực từ DB)
        //  → Lợi nhuận = TienBida + TienSanPham (toàn bộ doanh thu)
        // ══════════════════════════════════════════════════════════
        public (double TienBida, double TienSanPham) GetTienBidaVaTienSanPhamThangHienTai()
            => thongKeDAL.GetTienBidaVaTienSanPhamThangHienTai();

        // ══════════════════════════════════════════════════════════
        //  [MỚI] Snapshot để phát hiện dữ liệu mới thực sự
        //  Trả về: (SoHoaDon, NgayMoiNhat, SoBanHoatDong)
        //  UI gọi định kỳ, so sánh với snapshot trước → chỉ reload
        //  khi có thay đổi thực sự, không reload thừa.
        // ══════════════════════════════════════════════════════════
        public (int SoHoaDon, DateTime NgayMoiNhat, int SoBanHoatDong) GetSnapshotThayDoi()
            => thongKeDAL.GetSnapshotThayDoi();

        // ══════════════════════════════════════════════════════════
        //  BIỂU ĐỒ NGÀY – trả về ĐỦ mỗi ngày trong khoảng
        //  FIX: ngày không có đơn → doanh thu = 0, không bỏ sót
        // ══════════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetBieuDoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            var ketQua = new List<Dictionary<string, object>>();
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoNgay(tuNgay, denNgay);

            var dictDB = new Dictionary<DateTime, double>();
            foreach (var item in listRaw)
            {
                DateTime date = Convert.ToDateTime(item.NgayBan).Date;
                double doanhThu = Convert.ToDouble(item.DoanhThu);
                dictDB[date] = doanhThu;
            }

            for (DateTime d = tuNgay.Date; d <= denNgay.Date; d = d.AddDays(1))
            {
                double doanhThu = dictDB.ContainsKey(d) ? dictDB[d] : 0;
                double giaVon = doanhThu > 0 ? thongKeDAL.GetGiaVonTheoNgay(d) : 0;
                double loiNhuan = doanhThu - giaVon;

                ketQua.Add(new Dictionary<string, object>
                {
                    { "ngay_ban_label", d.ToString("dd/MM") },
                    { "doanh_thu",      doanhThu             },
                    { "loi_nhuan",      loiNhuan             }
                });
            }

            return ketQua;
        }

        // ══════════════════════════════════════════════════════════
        //  BIỂU ĐỒ THÁNG – trả về ĐỦ 12 tháng trong năm
        // ══════════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetBieuDoTheoThang(int nam)
        {
            var ketQua = new List<Dictionary<string, object>>();
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoThang(nam);

            var dictDB = new Dictionary<int, double>();
            foreach (var item in listRaw)
            {
                int thang = Convert.ToInt32(item.Thang);
                double doanhThu = Convert.ToDouble(item.DoanhThu);
                dictDB[thang] = doanhThu;
            }

            for (int thang = 1; thang <= 12; thang++)
            {
                double doanhThu = dictDB.ContainsKey(thang) ? dictDB[thang] : 0;
                double giaVon = doanhThu > 0 ? thongKeDAL.GetGiaVonTheoThang(thang, nam) : 0;
                double loiNhuan = doanhThu - giaVon;

                ketQua.Add(new Dictionary<string, object>
                {
                    { "thang_label", "Tháng " + thang },
                    { "doanh_thu",   doanhThu          },
                    { "loi_nhuan",   loiNhuan          }
                });
            }

            return ketQua;
        }
    }
}