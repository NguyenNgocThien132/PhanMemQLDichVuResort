using DDL;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmCapLaiMatKhau : Form
    {
        DLL_NhanVien nhanvien;
        public FrmCapLaiMatKhau()
        {
            InitializeComponent();
     
        }

        // Gửi lại mk vào email cho nhân viên 
        private void btnCapMatKhau_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text != "" && txtTaiKhoan.Text != "")
            {
                nhanvien = new DLL_NhanVien();
                if (nhanvien.IsExistEmail(txtEmail.Text) && nhanvien.IsExistTaiKhoan(txtTaiKhoan.Text))
                {
                    if(nhanvien.TrangThaiNhanVien(txtTaiKhoan.Text.Trim()))
                    {
                        string password = nhanvien.GetRandomPassword();

                        if (nhanvien.UpdatePassword(txtEmail.Text, password))
                        {
                            FrmGuiMail loader = new FrmGuiMail(txtEmail.Text, txtTaiKhoan.Text, password, true);
                            loader.ShowDialog();
                            MessageBox.Show(loader.Result, "Thông báo");
                            txtTaiKhoan.Text = "";
                            txtEmail.Text = "";
                            txtTaiKhoan.Focus();
                        }
                        else
                            MessageBox.Show("Không thực hiện được", "Thông báo");
                    }
                    else
                        MessageBox.Show("Tài khoản đã bị vô hiệu hóa!!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Email hoặc tài khoản không tồn tại trong hệ thống QL RESORT", "Thông báo");
                    txtTaiKhoan.Text = "";
                    txtEmail.Text = "";
                }         
            }
            else
                MessageBox.Show("Vui lòng nhập email hoặc tài khoản", "Thông báo");
        }

    }
}
