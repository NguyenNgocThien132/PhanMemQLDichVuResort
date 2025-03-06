using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DDL;

namespace GUI
{
    public partial class FrmDuyetPhong : Form
    {
        DLL_DuyetPhongOnline dllduyetphong = new DLL_DuyetPhongOnline();
        DLL_NhanVien nhanvien = new DLL_NhanVien();

        private string[] danhsachNV;
        private string str;
        private char separator = '|';
        private string taikhoan;
        public FrmDuyetPhong(string taikhoan)
        {
            this.taikhoan = taikhoan;
            InitializeComponent();
        }

        private void FrmDuyetPhong_Load(object sender, EventArgs e)
        {
            dtDuyetPhong.DataSource = dllduyetphong.DanhSachDatPhongOnline();
            LoadNhanVienDatPhong();
           
        }
        private void LoadNhanVienDatPhong()
        {
            str = nhanvien.LayIDNameNhanVien(taikhoan);
            if (str != "")
            {
                txtNhanVien.Text = str;
            }
        }
     
        private void btn_duyetphong_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn duyệt đơn đặt phòng này không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    string id_datphong = dtDuyetPhong.CurrentRow.Cells[0].Value.ToString();
                    string id_phong = dtDuyetPhong.CurrentRow.Cells[1].Value.ToString();
                    danhsachNV = str.Split(separator);
                    string manv = danhsachNV[0].Trim();
                    if (dllduyetphong.DuyetDatPhongOnline(id_datphong, manv))
                    {
                        if (dllduyetphong.CapNhapTrangThaiPhongOnline(id_phong))
                        {
                            MessageBox.Show("Duyệt phòng thành công!");
                            FrmDuyetPhong_Load(sender, e);
                        }
                    }
                }
                else
                    MessageBox.Show("Không thể thực hiện");

            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng chọn đơn cần duyệt");
            }
        }

        private void btnHuyPhong_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn hủy đơn đặt phòng này không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    string id_datphong = dtDuyetPhong.CurrentRow.Cells[0].Value.ToString();
                    if (dllduyetphong.HuyDatPhongOnline(id_datphong))
                    {
                        MessageBox.Show("Hủy đặt phòng thành công!");
                        FrmDuyetPhong_Load(sender, e);
                    }
                }
                else
                    MessageBox.Show("Không thể thực hiện"); 
            }
            catch (Exception)
            {

                MessageBox.Show("Vui lòng chọn đơn cần hủy");
            }     
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            FrmDuyetPhong_Load(sender, e);
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
