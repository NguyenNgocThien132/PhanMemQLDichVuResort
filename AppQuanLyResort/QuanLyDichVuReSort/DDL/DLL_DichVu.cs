using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
namespace DDL
{
  public  class DLL_DichVu
    {
      DAL_DichVu dichvu = new DAL_DichVu();

        public List<dichvu> DanhSachDichVu()
        {
            return dichvu.DanhSachDichVu();
        }
        public bool ThemDichVu(string madichvu, string tendv, int gia)
        {
            return dichvu.ThemDichVu(madichvu, tendv, gia);
        }
        public bool KiemTraKhoaChinh(string madv)
        {
            return dichvu.KiemTraKhoaChinh(madv);    
        }
        public bool XoaDichVu(string iddichvu)
        {
            return dichvu.XoaDichVu(iddichvu);
        }
        public bool SuaDichVu(string tendv, int giadv, string madv)
        {
            return dichvu.SuaDichVu(tendv, giadv, madv);
        }
        public List<int> GiaDichVu()
        {
            return dichvu.GiaDichVu();
        }
        public List<dichvu> LoadDichVuComboBoxGia(int gia)
        {
            return dichvu.LoadDichVuComboBoxGia(gia);
        }
        public List<dichvu> TimKiemDichVu(string ten)
        {
            return dichvu.TimKiemDichVu(ten);
        }
        public string[] DanhSachMavaTenDV()
        {
            return dichvu.DanhSachMavaTenDV();
        }
        public double LayGiaDichVu(string id)
        {
            return dichvu.LayGiaDichVu(id);
        }
        public bool KiemTraTonTaiMaDV(string ma)
        {
            return dichvu.KiemTraTonTaiMaDV(ma);
        }
        public string LayMaDichVuCuoiCung()
        {
            return dichvu.LayMaDichVuCuoiCung();
        }
    }
}
