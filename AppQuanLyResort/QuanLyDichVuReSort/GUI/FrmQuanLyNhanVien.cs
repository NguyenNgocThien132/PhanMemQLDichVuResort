using DAL;
using DDL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace GUI
{
    public partial class FrmQuanLyNhanVien : Form
    {
        private string role;
        private int vaitro;
        private string name;

        DLL_NhanVien nhanvien = new DLL_NhanVien();
        public FrmQuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void FrmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            SetValue(true, false);
        }
        private void LoadNhanVien()
        {
            dataNhanVien.DataSource = nhanvien.DanhSachNhanVien();

            dataNhanVien.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataNhanVien.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            cbbGioiTinh.Items.Clear();
            cbbGioiTinh.Items.Add("Nam");
            cbbGioiTinh.Items.Add("Nữ");
            cbbGioiTinh.Items.Add("Khác");
        }

        // kiểm tra Email có giá trị
        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool KiemTraSoDienThoai(string soDienThoai)
        {

            string pattern = @"^\d{10}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(soDienThoai);
        }

        //Thiết lặt thao tác
        private void SetValue(bool param, bool isLoad)
        {
            txtEmail.ReadOnly = false;
            txtNgaysinh.Text = null;
            txtEmail.Text = null;
            txtTaiKhoan.Text = null;
            txtSDT.Text = null;
            btnThem.Enabled = true;
            txtHoTen.Text = null;
            txtTimKiem.Text = null;
            LoadNhanVien();
            txtHoTen.Focus();

            if (isLoad) // true
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
            else // false
            {
                btnSua.Enabled = !param;// !param == true
                btnXoa.Enabled = !param;// !param == true
            }
            rad_letan.Checked = true;
        }
        private void dataNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                btnSua.Enabled = btnXoa.Enabled = true;
                if (dataNhanVien.Rows.Count > 0)
                {
                    txtMa.Text = dataNhanVien.CurrentRow.Cells[0].Value.ToString();
                    txtHoTen.Text = dataNhanVien.CurrentRow.Cells[1].Value.ToString();
                    string ngaysinhString = dataNhanVien.CurrentRow.Cells[2].Value.ToString();
                    DateTime ngaysinh = DateTime.Parse(ngaysinhString);
                    txtNgaysinh.Text = ngaysinh.ToString("dd/MM/yyyy");

                    txtSDT.Text = dataNhanVien.CurrentRow.Cells[3].Value.ToString();
                    cbbGioiTinh.SelectedItem = dataNhanVien.CurrentRow.Cells[4].Value.ToString();
                    txtEmail.Text = dataNhanVien.CurrentRow.Cells[5].Value.ToString();
                    txtTaiKhoan.Text = dataNhanVien.CurrentRow.Cells[6].Value.ToString();
                    role = dataNhanVien.CurrentRow.Cells[7].Value.ToString();

                    if (role == "Lễ Tân")
                    {
                        rad_letan.Checked = true;
                    }
                    else if (role == "Thu Ngân")
                    {
                        ra_thungan.Checked = true;
                    }
                    else
                        ra_quanly.Checked = true;

                }
            }
            catch (Exception) { }

        }
        private void MsgBox(string message, bool isError = false)
        {
            if (isError)
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa nhân viên này không", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                string ma = dataNhanVien.CurrentRow.Cells[0].Value.ToString();
                if (nhanvien.XoaNhanVien(ma))
                {
                    SetValue(true, false);
                    MessageBox.Show("Xóa nhân viên thành công");
                    LoadNhanVien();
                }
                else
                    MsgBox("Nhân viên đã có đóng góp trong resort không thể xóa. Vui lòng vô hiệu hóa tài khoản nếu không dùng!!!", true);
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string macuoi = nhanvien.LayMaNhanVienCuoi();
                string mamoi = null;
                string[] parts = macuoi.Split('_');

                if (parts.Length == 2)
                {
                    string makhcu = parts[0];
                    int soThuTuCu = 0;

                    if (int.TryParse(parts[1], out soThuTuCu))
                    {
                        bool manhanvientontai;
                        do
                        {
                            int soThuTuMoi = soThuTuCu + 1;
                            mamoi = makhcu + "_" + soThuTuMoi;

                            manhanvientontai = nhanvien.KiemTraTonTaiMaNhanVien(mamoi);

                            if (manhanvientontai)
                            {
                                soThuTuCu++;
                            }
                        } while (manhanvientontai);
                    }
                }

                if (txtTaiKhoan.Text != "" && txtEmail.Text != "" && txtHoTen.Text != ""
                  && txtSDT.Text != "" && txtNgaysinh.Text != "")
                {

                    if (nhanvien.KiemTraTonTaiEmail(txtEmail.Text))
                    {
                        MsgBox("Email đã tồn tại, vui lòng xem lại!", true);
                        return;
                    }
                    if (nhanvien.KiemTraTonTaiTaiKhoan(txtTaiKhoan.Text))
                    {
                        MsgBox("Tài khoản đã tồn tại, vui lòng xem lại!", true);
                        return;
                    }
                    if (nhanvien.KiemTraTonTaiSDT(txtSDT.Text))
                    {
                        MsgBox("Số điện thoại đã tồn tại, vui lòng xem lại!", true);
                        return;
                    }

                    if (IsValidEmail(txtEmail.Text) && KiemTraSoDienThoai(txtSDT.Text))
                    {

                        if (rad_letan.Checked)
                        { vaitro = 1; }
                        else if (ra_thungan.Checked)
                        { vaitro = 2; }
                        else
                            vaitro = 3;

                        string password = nhanvien.GetRandomPassword();
                        DTO_NhanVien dtonhanvien = new DTO_NhanVien
                              (
                              mamoi,
                              txtHoTen.Text,
                              DateTime.Parse(txtNgaysinh.Text),
                              txtSDT.Text,
                              cbbGioiTinh.SelectedItem.ToString(),
                              txtEmail.Text,
                              txtTaiKhoan.Text,
                              password,
                              vaitro
                              );

                        if (nhanvien.ThemNhanVien(dtonhanvien))
                        {
                            SetValue(false, true);

                            LoadNhanVien();
                            FrmGuiMail sendMail = new FrmGuiMail(dtonhanvien.Email, dtonhanvien.Tendangnhap, password);
                            sendMail.ShowDialog();
                            MsgBox(sendMail.Result);
                        }
                        else
                            MsgBox("Không thêm nhân viên được!", true);
                    }
                    else MsgBox("Email hoặc SĐT không đúng định dạng !", true);

                }
                else MsgBox("Thiếu trường thông tin!", true);
            }
            catch (Exception)
            {
                MsgBox("Lỗi xem lại!", true);
            }

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTaiKhoan.Text != "" && txtEmail.Text != "" && txtHoTen.Text != ""
            && txtSDT.Text != "")
                {
                    if (rad_letan.Checked)
                    {
                        vaitro = 1;
                    }
                    else if (ra_thungan.Checked)
                    {
                        vaitro = 2;
                    }
                    else
                        vaitro = 3;

                    string manv = dataNhanVien.CurrentRow.Cells[0].Value.ToString();

                    if (manv == null)
                    { manv = txtMa.Text.Trim(); }

                    DTO_NhanVien dtoEmployee = new DTO_NhanVien
                         (
                              manv,
                              txtHoTen.Text,
                              DateTime.Parse(txtNgaysinh.Text),
                              txtSDT.Text,
                              cbbGioiTinh.SelectedItem.ToString(),
                              txtEmail.Text,
                              txtTaiKhoan.Text,
                              vaitro
                              );
                    if (nhanvien.SuaNhanVien(dtoEmployee))
                    {
                        SetValue(true, false);

                        MessageBox.Show("Sửa nhân viên thành công");
                        LoadNhanVien();
                    }
                    else
                        MsgBox("Sửa nhân viên không thành công!", true);
                }
                else MsgBox("Thiếu trường thông tin!", true);
            }
            catch (Exception)
            {

                MsgBox("Lỗi!", true);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            SetValue(false, true);

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            name = txtTimKiem.Text.Trim();
            if (name == "")
            {
                FrmQuanLyNhanVien_Load(sender, e);
                txtTimKiem.Focus();
            }
            else
            {
                DataTable data = nhanvien.TimKiemNhanVien(name);
                dataNhanVien.DataSource = data;
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void btnNhanVienKhongHoatDong_Click(object sender, EventArgs e)
        {
            if (new FrmNhanVienKhongHoatDong().ShowDialog(this) == DialogResult.OK)
            {
                LoadNhanVien();
            }    
           
        }


    }
}
