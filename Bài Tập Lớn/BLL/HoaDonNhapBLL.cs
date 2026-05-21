using Bài_Tập_Lớn.DTO;
using System;
using Bài_Tập_Lớn.DAL;
using System.Collections.Generic;
public class HoaDonNhapBLL
{
    private readonly HoaDonNhapDAL hoaDonNhapDAL = new HoaDonNhapDAL();

    public string SinhMaMoi()
    {
        return hoaDonNhapDAL.SinhMaMoi();
    }

    public List<HoaDonNhapDTO> LayTatCaHoaDonNhap()
    {
        return hoaDonNhapDAL.LayTatCaHoaDonNhap();
    }

    public HoaDonNhapDTO TimTheoMaHDN(string maHDN)
    {
        if (string.IsNullOrWhiteSpace(maHDN))
        {
            throw new Exception("Mã hóa đơn nhập không được để trống!");
        }

        return hoaDonNhapDAL.TimTheoMaHDN(maHDN);
    }

    public bool ThemHoaDonNhap(HoaDonNhapDTO hdn)
    {
        if (hdn == null)
        {
            throw new Exception("Dữ liệu hóa đơn nhập không hợp lệ!");
        }

        if (string.IsNullOrWhiteSpace(hdn.MaHDN))
        {
            throw new Exception("Mã hóa đơn nhập không được để trống!");
        }

        if (string.IsNullOrWhiteSpace(hdn.MaNCC))
        {
            throw new Exception("Mã nhà cung cấp không được để trống!");
        }

        return hoaDonNhapDAL.ThemHoaDonNhap(hdn);
    }

    public bool CapNhatHoaDonNhap(HoaDonNhapDTO hdn)
    {
        if (hdn == null)
        {
            throw new Exception("Dữ liệu hóa đơn nhập không hợp lệ!");
        }

        if (string.IsNullOrWhiteSpace(hdn.MaHDN))
        {
            throw new Exception("Mã hóa đơn nhập không được để trống!");
        }

        return hoaDonNhapDAL.CapNhatHoaDonNhap(hdn);
    }

    public bool XoaHoaDonNhap(string maHDN)
    {
        if (string.IsNullOrWhiteSpace(maHDN))
        {
            throw new Exception("Mã hóa đơn nhập không được để trống!");
        }

        return hoaDonNhapDAL.XoaHoaDonNhap(maHDN);
    }
}