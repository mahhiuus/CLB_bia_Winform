using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;

namespace Bài_Tập_Lớn.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL thongKeDAL = new ThongKeDAL();

        // ══════════════════════════════════════════════════════════
        //  CARDS
        // ══════════════════════════════════════════════════════════
        public double GetDoanhThuThangHienTai() => thongKeDAL.GetDoanhThuThangHienTai();
        public int GetSoHoaDonThangHienTai() => thongKeDAL.GetSoHoaDonThangHienTai();
        public int GetKhachHangMoiThangHienTai() => thongKeDAL.GetKhachHangMoiThangHienTai();
        public int GetSoBanDangHoatDong() => thongKeDAL.GetSoBanDangHoatDong();
        public double GetGiaVonThangHienTai() => thongKeDAL.GetGiaVonThangHienTai();

        public (double TienBida, double TienSanPham) GetTienBidaVaTienSanPhamThangHienTai()
            => thongKeDAL.GetTienBidaVaTienSanPhamThangHienTai();

        public (int SoHoaDon, DateTime NgayMoiNhat, int SoBanHoatDong) GetSnapshotThayDoi()
            => thongKeDAL.GetSnapshotThayDoi();

        // ══════════════════════════════════════════════════════════
        //  BIỂU ĐỒ NGÀY
        //  [SỬA] Trả thêm tien_bida, tien_san_pham để UI tính LN
        //        LN = tien_bida - tien_san_pham
        // ══════════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetBieuDoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            var ketQua = new List<Dictionary<string, object>>();
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoNgay(tuNgay, denNgay);

            // index theo ngày để tra nhanh
            var dictDT = new Dictionary<DateTime, double>();
            var dictBida = new Dictionary<DateTime, double>();
            var dictSP = new Dictionary<DateTime, double>();

            foreach (var item in listRaw)
            {
                DateTime date = Convert.ToDateTime(item.NgayBan).Date;
                dictDT[date] = Convert.ToDouble(item.DoanhThu);
                dictBida[date] = Convert.ToDouble(item.TienBida);
                dictSP[date] = Convert.ToDouble(item.TienSanPham);
            }

            for (DateTime d = tuNgay.Date; d <= denNgay.Date; d = d.AddDays(1))
            {
                double dt = dictDT.ContainsKey(d) ? dictDT[d] : 0;
                double tb = dictBida.ContainsKey(d) ? dictBida[d] : 0;
                double ts = dictSP.ContainsKey(d) ? dictSP[d] : 0;

                ketQua.Add(new Dictionary<string, object>
                {
                    { "ngay_ban_label", d.ToString("dd/MM") },
                    { "doanh_thu",      dt                  },
                    { "tien_bida",      tb                  },   // [MỚI]
                    { "tien_san_pham",  ts                  }    // [MỚI]
                });
            }

            return ketQua;
        }

        // ══════════════════════════════════════════════════════════
        //  BIỂU ĐỒ THÁNG
        //  [SỬA] Trả thêm tien_bida, tien_san_pham
        // ══════════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetBieuDoTheoThang(int nam)
        {
            var ketQua = new List<Dictionary<string, object>>();
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoThang(nam);

            var dictDT = new Dictionary<int, double>();
            var dictBida = new Dictionary<int, double>();
            var dictSP = new Dictionary<int, double>();

            foreach (var item in listRaw)
            {
                int thang = Convert.ToInt32(item.Thang);
                dictDT[thang] = Convert.ToDouble(item.DoanhThu);
                dictBida[thang] = Convert.ToDouble(item.TienBida);
                dictSP[thang] = Convert.ToDouble(item.TienSanPham);
            }

            for (int thang = 1; thang <= 12; thang++)
            {
                double dt = dictDT.ContainsKey(thang) ? dictDT[thang] : 0;
                double tb = dictBida.ContainsKey(thang) ? dictBida[thang] : 0;
                double ts = dictSP.ContainsKey(thang) ? dictSP[thang] : 0;

                ketQua.Add(new Dictionary<string, object>
                {
                    { "thang_label",   "Tháng " + thang },
                    { "doanh_thu",     dt               },
                    { "tien_bida",     tb               },   // [MỚI]
                    { "tien_san_pham", ts               }    // [MỚI]
                });
            }

            return ketQua;
        }

        // ══════════════════════════════════════════════════════════
        //  [MỚI] BIỂU ĐỒ NĂM – các năm có dữ liệu trong DB
        // ══════════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetBieuDoTheoNam()
        {
            var ketQua = new List<Dictionary<string, object>>();
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoNam();

            foreach (var item in listRaw)
            {
                double dt = Convert.ToDouble(item.DoanhThu);
                double tb = Convert.ToDouble(item.TienBida);
                double ts = Convert.ToDouble(item.TienSanPham);

                ketQua.Add(new Dictionary<string, object>
                {
                    { "nam_label",     item.Nam.ToString() },
                    { "doanh_thu",     dt                  },
                    { "tien_bida",     tb                  },
                    { "tien_san_pham", ts                  }
                });
            }

            return ketQua;
        }

        // ══════════════════════════════════════════════════════════
        //  [MỚI] TOP MÁY DOANH THU CAO NHẤT THÁNG HIỆN TẠI
        // ══════════════════════════════════════════════════════════
        public List<(string TenMay, double DoanhThu)> GetTopMayDoanhThu(int top = 3)
        {
            var result = new List<(string, double)>();
            var listRaw = thongKeDAL.GetTopMayDoanhThu(top);

            foreach (var item in listRaw)
                result.Add((item.TenMay.ToString(), Convert.ToDouble(item.DoanhThu)));

            return result;
        }
    }
}