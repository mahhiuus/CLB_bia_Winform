using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class PhienChoiBLL
    {
        private readonly PhienChoiDAL phienChoiDAL = new PhienChoiDAL();

        public string SinhMaMoi()
        {
            return phienChoiDAL.SinhMaMoi();
        }

        public List<PhienChoiDTO> LayTatCaPhien()
        {
            return phienChoiDAL.LayTatCaPhien();
        }

        public PhienChoiDTO TimTheoMaPhien(string maPhien)
        {
            if (string.IsNullOrWhiteSpace(maPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            return phienChoiDAL.TimTheoMaPhien(maPhien);
        }

        public List<PhienChoiDTO> LayPhienTheoBan(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            return phienChoiDAL.LayPhienTheoBan(maBan);
        }

        public PhienChoiDTO TimPhienDangChoiTheoBan(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            return phienChoiDAL.TimPhienDangChoiTheoBan(maBan);
        }

        public bool ThemPhien(PhienChoiDTO phien)
        {
            if (phien == null)
            {
                throw new Exception("Dữ liệu phiên chơi không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(phien.MaPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(phien.MaBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            if (phien.ThoiGianBatDau == DateTime.MinValue)
            {
                throw new Exception("Thời gian bắt đầu không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(phien.TrangThai))
            {
                throw new Exception("Trạng thái phiên không được để trống!");
            }

            return phienChoiDAL.ThemPhien(phien);
        }

        public bool CapNhatPhien(PhienChoiDTO phien)
        {
            if (phien == null)
            {
                throw new Exception("Dữ liệu phiên chơi không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(phien.MaPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            return phienChoiDAL.CapNhatPhien(phien);
        }

        public bool KetThucPhien(string maPhien, DateTime thoiGianKetThuc)
        {
            if (string.IsNullOrWhiteSpace(maPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            if (thoiGianKetThuc == DateTime.MinValue)
            {
                throw new Exception("Thời gian kết thúc không hợp lệ!");
            }

            return phienChoiDAL.KetThucPhien(maPhien, thoiGianKetThuc);
        }

        public bool XoaPhien(string maPhien)
        {
            if (string.IsNullOrWhiteSpace(maPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            return phienChoiDAL.XoaPhien(maPhien);
        }
        public PhienChoiDTO LayPhienDangChoi(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan))
                throw new Exception("Mã bàn không được để trống!");

            return phienChoiDAL.TimPhienDangChoiTheoBan(maBan);
        }
    }
}