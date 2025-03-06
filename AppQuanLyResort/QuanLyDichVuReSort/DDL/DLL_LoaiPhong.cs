using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
namespace DDL
{
   public class DLL_LoaiPhong
    {
       DAL_LoaiPhong loaiphong = new DAL_LoaiPhong();

        public DataTable DanhSachLoaiPhong()
        {
            return loaiphong.DanhSachLoaiPhong();
        }
        public DataTable DanhSachLoaiPhongTenVaGia()
        {
            return loaiphong.DanhSachLoaiPhongTenVaGia();
        }
        public string[] MaTenLoaiPhong()
        {
            return loaiphong.MaTenLoaiPhong();
        }
        public DataTable TimKiemLoaiPhong(string tenphong)
        {
            return loaiphong.TimKiemLoaiPhong(tenphong);
        }
        public DataTable TimKiemLoaiPhong2(string tenphong)
        {
            return loaiphong.TimKiemLoaiPhong2(tenphong);
        }
        public DataTable TimKiemTheoGia(int gia1, int gia2)
        {
            return loaiphong.TimKiemTheoGia(gia1, gia2);
        }
    }
}
