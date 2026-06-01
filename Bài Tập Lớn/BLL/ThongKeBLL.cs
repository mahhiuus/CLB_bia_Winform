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

        // ══════════════════════════════════════════════════════════
        //  [MỚI] Lợi Nhuận = TienBida + LãiSanPham
        //  LãiSanPham = SUM((don_gia_ban - don_gia_von) * so_luong_ban)
        // ══════════════════════════════════════════════════════════
        public double GetLoiNhuanThangHienTai()
        {
            return thongKeDAL.GetLoiNhuanThangHienTai();
        }

        public (int SoHoaDon, DateTime NgayMoiNhat, int SoBanHoatDong) GetSnapshotThayDoi()
            => thongKeDAL.GetSnapshotThayDoi();

        // ══════════════════════════════════════════════════════════
        //  BIỂU ĐỒ NGÀY
        //  Dictionary bổ sung key "lai_san_pham" để UI tính đúng
        //  Lợi Nhuận = tien_bida + lai_san_pham
        // ══════════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetBieuDoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            var ketQua = new List<Dictionary<string, object>>();

            // Lấy doanh thu / tien_bida / tien_san_pham theo ngày
            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoNgay(tuNgay, denNgay);
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

            // Lấy lãi sản phẩm theo ngày (tính từ chi_tiet_hoa_don_ban)
            var listLai = thongKeDAL.GetLaiSanPhamTheoNgay(tuNgay, denNgay);
            var dictLai = new Dictionary<DateTime, double>();
            foreach (var item in listLai)
            {
                DateTime date = Convert.ToDateTime(item.NgayBan).Date;
                dictLai[date] = Convert.ToDouble(item.LaiSanPham);
            }

            for (DateTime d = tuNgay.Date; d <= denNgay.Date; d = d.AddDays(1))
            {
                double dt = dictDT.ContainsKey(d) ? dictDT[d] : 0;
                double tb = dictBida.ContainsKey(d) ? dictBida[d] : 0;
                double ts = dictSP.ContainsKey(d) ? dictSP[d] : 0;
                double lai = dictLai.ContainsKey(d) ? dictLai[d] : 0;

                ketQua.Add(new Dictionary<string, object>
                {
                    { "ngay_ban_label", d.ToString("dd/MM") },
                    { "doanh_thu",      dt                  },
                    { "tien_bida",      tb                  },
                    { "tien_san_pham",  ts                  },
                    { "lai_san_pham",   lai                 }   // [MỚI]
                });
            }

            return ketQua;
        }

        // ══════════════════════════════════════════════════════════
        //  BIỂU ĐỒ THÁNG
        //  Dictionary bổ sung key "lai_san_pham"
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

            // Lấy lãi sản phẩm theo tháng
            var listLai = thongKeDAL.GetLaiSanPhamTheoThang(nam);
            var dictLai = new Dictionary<int, double>();
            foreach (var item in listLai)
            {
                int thang = Convert.ToInt32(item.Thang);
                dictLai[thang] = Convert.ToDouble(item.LaiSanPham);
            }

            for (int thang = 1; thang <= 12; thang++)
            {
                double dt = dictDT.ContainsKey(thang) ? dictDT[thang] : 0;
                double tb = dictBida.ContainsKey(thang) ? dictBida[thang] : 0;
                double ts = dictSP.ContainsKey(thang) ? dictSP[thang] : 0;
                double lai = dictLai.ContainsKey(thang) ? dictLai[thang] : 0;

                ketQua.Add(new Dictionary<string, object>
                {
                    { "thang_label",   "Tháng " + thang },
                    { "doanh_thu",     dt               },
                    { "tien_bida",     tb               },
                    { "tien_san_pham", ts               },
                    { "lai_san_pham",  lai              }   // [MỚI]
                });
            }

            return ketQua;
        }

        // ══════════════════════════════════════════════════════════
        //  BIỂU ĐỒ NĂM
        //  Dictionary bổ sung key "lai_san_pham"
        // ══════════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetBieuDoTheoNam()
        {
            var ketQua = new List<Dictionary<string, object>>();

            var listRaw = thongKeDAL.GetDuLieuBieuDoTheoNam();

            // Lấy lãi sản phẩm theo năm
            var listLai = thongKeDAL.GetLaiSanPhamTheoNam();
            var dictLai = new Dictionary<int, double>();
            foreach (var item in listLai)
            {
                int nam = Convert.ToInt32(item.Nam);
                dictLai[nam] = Convert.ToDouble(item.LaiSanPham);
            }

            foreach (var item in listRaw)
            {
                double dt = Convert.ToDouble(item.DoanhThu);
                double tb = Convert.ToDouble(item.TienBida);
                double ts = Convert.ToDouble(item.TienSanPham);
                int nam = Convert.ToInt32(item.Nam);
                double lai = dictLai.ContainsKey(nam) ? dictLai[nam] : 0;

                ketQua.Add(new Dictionary<string, object>
                {
                    { "nam_label",     nam.ToString() },
                    { "doanh_thu",     dt             },
                    { "tien_bida",     tb             },
                    { "tien_san_pham", ts             },
                    { "lai_san_pham",  lai            }   // [MỚI]
                });
            }

            return ketQua;
        }

        // ══════════════════════════════════════════════════════════
        //  TOP MÁY DOANH THU CAO NHẤT THÁNG HIỆN TẠI
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