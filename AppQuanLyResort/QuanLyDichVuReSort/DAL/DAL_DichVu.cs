﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAL
{
   public class DAL_DichVu: DbConnect
    {
        QLDVRSDataContext qlrs = new QLDVRSDataContext();

        //Danh sách dịch vụ
        public List<dichvu> DanhSachDichVu()
        {
            List<dichvu> list = new List<dichvu>();
            var dvs = from dv in qlrs.dichvus
                      select dv;
            list = dvs.ToList();
            return list;
        }


        //Lấy dịch vụ cuối cùng
        public string LayMaDichVuCuoiCung()
        {
            var dichvucuoicung = qlrs.dichvus.OrderByDescending(dv => dv.id_dichvu).FirstOrDefault();

            if (dichvucuoicung != null)
            {
                return dichvucuoicung.id_dichvu;
            }
            else
                return "Không có dịch vụ";
        }

        //Kiểm tra mã dịch vụ có tồn tại khôngS
        public bool KiemTraTonTaiMaDV(string ma)
        {
            var dvs = from dv in qlrs.dichvus
                             where dv.id_dichvu.Contains(ma)
                             select dv;

            bool dichvutontai = dvs.Any();
            return dichvutontai;

        }
        public bool ThemDichVu(string iddichvu, string tendv, int giadv)
        {
            dichvu dv = new dichvu();
            dv.id_dichvu = iddichvu;
            dv.ten_dichvu = tendv;
            dv.gia = giadv;
            try
            {
                qlrs.dichvus.InsertOnSubmit(dv);
                qlrs.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool KiemTraKhoaChinh(string madv)
        {
            dichvu dichvu = qlrs.dichvus.Where(row => row.id_dichvu == madv).FirstOrDefault();
            if (dichvu != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool XoaDichVu(string iddichvu)
        {
            dichvu dv = qlrs.dichvus.Where(row => row.id_dichvu == iddichvu).FirstOrDefault();
            bool kq = false;
            if (dv != null)
            {
                try
                {
                    qlrs.dichvus.DeleteOnSubmit(dv);
                    qlrs.SubmitChanges();
                    kq = true;
                }
                catch (Exception)
                {
                    kq = false;
                }
            }
            return kq;
        }

        public bool SuaDichVu(string tendv, int giadv, string madv)
        {
            dichvu dv = qlrs.dichvus.Where(row => row.id_dichvu == madv).FirstOrDefault();
            bool kq = false;
            if (dv != null)
            {
                try
                {
                    dv.ten_dichvu = tendv;
                    dv.gia = giadv;
                    qlrs.SubmitChanges();
                    kq = true;
                }
                catch (Exception)
                {
                    kq = false;
                }
            }
            return kq;
        }

        public List<int> GiaDichVu()
        {
            List<int> list = qlrs.dichvus.Select(dv => dv.gia).ToList();
            return list;
        }
        public List<dichvu> LoadDichVuComboBoxGia(int gia)
        {
            List<dichvu> list = new List<dichvu>();
            var dvs = from dv in qlrs.dichvus where dv.gia == gia
                      select dv;
            list = dvs.ToList();
            return list;
        }

        public List<dichvu> TimKiemDichVu(string ten)
        {
            List<dichvu> list = qlrs.dichvus.Where(row=>row.ten_dichvu.Contains(ten)).ToList();
            
            return list;
        }

        // Danh sách mã, tên dịch vụ
        public string[] DanhSachMavaTenDV()
        {
            try
            {
                _conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DanhSachMavaTenDV";
                List<string> list = new List<string>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                return list.ToArray();
            }
            finally
            {
                _conn.Close();
            }
        }
        // Lấy giá dịch vụ
        public double LayGiaDichVu(string id)
        {
            try
            {
                _conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "LayGiaDichVu";
                cmd.Parameters.AddWithValue("id_dichvu", id);
                return Convert.ToDouble(cmd.ExecuteScalar());
            }
            finally
            {
                _conn.Close();
            }
        }
    }
}
