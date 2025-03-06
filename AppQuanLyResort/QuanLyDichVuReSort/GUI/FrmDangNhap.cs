using DDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmDangNhap : Form
    {
        DLL_NhanVien nhanvien = new DLL_NhanVien();

        string strtaikhoan, strmatkhau;
        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {
           
            if (Properties.Settings.Default.isSave)
            {
                strtaikhoan = txt_taikhoan.Text.Trim();
                strmatkhau = txt_matkhau.Text.Trim();
                strtaikhoan = Properties.Settings.Default.taikhoan;
                strmatkhau = Properties.Settings.Default.matkhau;
                check.Checked = true;
            }
          
        }
        private void ResetThongTin()
        {
            txt_taikhoan.Text = "";
            txt_matkhau.Text = "";
            txt_taikhoan.Focus();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_taikhoan.Text) && String.IsNullOrEmpty(txt_matkhau.Text))
            {
                MessageBox.Show("TÀI KHOẢN VÀ MẬT KHẨU KHÔNG ĐƯỢC ĐỂ TRỐNG");
                return;
            }
            if (String.IsNullOrEmpty(txt_taikhoan.Text))
            {
                MessageBox.Show("TÀI KHOẢN KHÔNG ĐƯỢC ĐỂ TRỐNG");
                return;
            }
            if (String.IsNullOrEmpty(txt_matkhau.Text))
            {
                MessageBox.Show("MẬT KHẨU KHÔNG ĐƯỢC ĐỂ TRỐNG");
                return;
            }

            if (txt_taikhoan.Text != "" && txt_matkhau.Text != "")
            {
                if (nhanvien.Login(txt_taikhoan.Text, txt_matkhau.Text))
                {
                    Properties.Settings.Default.isSave = check.Checked;
                    if (check.Checked)
                    {
                        Properties.Settings.Default.taikhoan = txt_taikhoan.Text;
                        Properties.Settings.Default.matkhau = txt_matkhau.Text;
                    }
                    Properties.Settings.Default.Save();
                    if(nhanvien.TrangThaiNhanVien(txt_taikhoan.Text.Trim()))
                    {
                        FrmMain fMain = new FrmMain(txt_taikhoan.Text);
                        MessageBox.Show("CHÀO MỪNG BẠN ĐÃ ĐĂNG NHẬP VÀO RESORT!!!");
                        this.Hide();
                        fMain.ShowDialog();
                        this.Show();
                    }  
                    else
                        MessageBox.Show("TÀI KHOẢN CỦA BẠN ĐÃ BỊ VÔ HIỆU HÓA, VUI LÒNG LIÊN HỆ NGƯỜI QUẢN LÝ ĐỂ BIẾT THÊM CHI TIẾT!!!");
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResetThongTin();
                }
            }
        }

        private void btn_hide_Click(object sender, EventArgs e)
        {
            txt_matkhau.PasswordChar = '\0';
            btn_show.Visible = true;
            btn_hide.Visible = false;
        }
        private void btn_show_Click(object sender, EventArgs e)
        {       
            txt_matkhau.PasswordChar = '*';
            btn_show.Visible = false;
            btn_hide.Visible = true;
        }

        //Di chuyển form qua panel
        private void guna2GradientPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                guna2GradientPanel1.Capture = false;
                const int WM_NCLBUTTONDOWN = 0x00A1;
                const int HTCAPTION = 2;
                Message msg =
                    Message.Create(this.Handle, WM_NCLBUTTONDOWN,
                        new IntPtr(HTCAPTION), IntPtr.Zero);
                this.DefWndProc(ref msg);
            }
        }

        //Link cấp lại mật khẩu cho nhân viên
        private void linkLabel_foret_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FrmCapLaiMatKhau().ShowDialog();
        }
    }
}
