using DAL;
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
    public partial class FrmQuanLyDichVu : Form
    {
        DLL_DichVu dichvu = new DLL_DichVu();
        public FrmQuanLyDichVu()
        {
            InitializeComponent();
        }

        private void FrmQuanLyDichVu_Load(object sender, EventArgs e)
        {
            LoadDichVu();
            LoadCbbGiaDichVu();
            SetValue(false, true);
        }
        private void LoadCbbGiaDichVu()
        {
            List<int> giaDichVu = dichvu.GiaDichVu();
            giaDichVu.Insert(0, 1);
          
        }
        private void LoadDichVu()
        {
            dataDichVu.DataSource = dichvu.DanhSachDichVu();
        }

        //Thiết lặt thao tác
        private void SetValue(bool param, bool isLoad)
        {
            txtMaDV.Text = null;
            txtTenDV.Text = null;
            txtGiaDV.Text = null;

            btnThem.Enabled = true;
            LoadDichVu();
            txtTenDV.Focus();

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

        }
        private void MsgBox(string message, bool isError = false)
        {
            if (isError)
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void dataDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataDichVu.Rows.Count > 0)
            {
                btnSua.Enabled = btnXoa.Enabled = true;
                this.txtMaDV.Text = dataDichVu.CurrentRow.Cells[0].Value.ToString();
                this.txtTenDV.Text = dataDichVu.CurrentRow.Cells[1].Value.ToString();
                this.txtGiaDV.Text = dataDichVu.CurrentRow.Cells[2].Value.ToString();  
            }    
           
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            SetValue(false, true);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string macuoi = dichvu.LayMaDichVuCuoiCung();
            string mamoi = null;

            string[] parts = macuoi.Split('_');

            if (parts.Length == 2)
            {
                string makhcu = parts[0];
                int soThuTuCu = 0;

                if (int.TryParse(parts[1], out soThuTuCu))
                {
                    bool madichvutrung;
                    do
                    {
                        int soThuTuMoi = soThuTuCu + 1;
                        mamoi = makhcu + "_" + soThuTuMoi;

                        madichvutrung = dichvu.KiemTraTonTaiMaDV(mamoi);

                        if (madichvutrung)
                        {
                            soThuTuCu++;
                        }
                    } while (madichvutrung);
                }
            }
            int giadv = 0;

            if (string.IsNullOrWhiteSpace(txtGiaDV.Text))
            {
                MessageBox.Show("Giá không được để trống!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenDV.Text))
            {
                MessageBox.Show("Tên dịch vụ không được để trống!");
                return;
            }

            if (!int.TryParse(txtGiaDV.Text, out giadv))
            {
                MessageBox.Show("Giá phải là một số nguyên!");
                return;
            }

            if (macuoi == "Không có dịch vụ")
            {
                macuoi = "DV_1";
                if (dichvu.ThemDichVu(macuoi, txtTenDV.Text, int.Parse(txtGiaDV.Text)))
                {
                    MessageBox.Show("Thêm thành công dịch vụ!");
                    LoadDichVu();
                    SetValue(false, true);
                }    
                else
                    MsgBox("Không thể thêm dịch vụ được!", true);

            }
            else
            {
                if (dichvu.ThemDichVu(mamoi, txtTenDV.Text, int.Parse(txtGiaDV.Text)))
                {
                    MessageBox.Show("Thêm thành công dịch vụ!");
                    LoadDichVu();
                    SetValue(false, true);
                }
                else
                    MsgBox("Không thể thêm dịch vụ được!", true);
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn có muốn xóa dịch vụ này không", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                string ma = dataDichVu.CurrentRow.Cells[0].Value.ToString();
                if (dichvu.XoaDichVu(ma))
                {
                    SetValue(true, false);
                    MessageBox.Show("Xóa dịch vụ thành công");
                    LoadDichVu();
                }
                else
                    MsgBox("Dịch vụ đang sử dụng, không thể xóa", true);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            int giadv = 0;

            // Check if the price textbox is empty or not a valid integer
            if (string.IsNullOrWhiteSpace(txtGiaDV.Text))
            {
                MessageBox.Show("Giá không được để trống!");
                return;
            }
            if (!int.TryParse(txtGiaDV.Text, out giadv))
            {
                MessageBox.Show("Giá phải là một số nguyên!");
                return;
            }
            string matb = dataDichVu.CurrentRow.Cells[0].Value.ToString();

            if (matb == null)
            { matb = txtMaDV.Text.Trim(); }

            if (dichvu.SuaDichVu(txtTenDV.Text, int.Parse(txtGiaDV.Text), matb))
            {
                SetValue(true, false);
                MessageBox.Show("Sửa thành công dịch vụ!");
                LoadDichVu();
            }
            else
                MsgBox("Sửa dịch vụ không thành công!", true);
        }

       
        private void txtTimKiemTen_TextChanged(object sender, EventArgs e)
        {
            string ten = txtTimKiemTen.Text;
            if (ten != "")
            {
                LoadDichVu();
            }
            else
            {
                dataDichVu.DataSource = dichvu.TimKiemDichVu(ten);
            }
        }

        private void txtGiaDV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
