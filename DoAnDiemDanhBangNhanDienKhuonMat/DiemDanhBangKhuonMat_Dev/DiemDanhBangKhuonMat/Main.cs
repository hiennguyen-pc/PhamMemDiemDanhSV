using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiemDanhBangKhuonMat
{
    public partial class Main : Form
    {
        public Main()
        {

           

            InitializeComponent();
        }
        public void funData(TextBox txtForm1)
        {
            label3.Text = txtForm1.Text;
            string str2 = "Admin";
            if (String.Compare(label3.Text, str2, true) != 0)
            {
                button2.Enabled = false;
                button2.Visible = false;
                button3.Visible = false;
                button3.Enabled = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ThemSinhVien add = new ThemSinhVien();
            add.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiemDanhSinhVien detect = new DiemDanhSinhVien();
            detect.Show();
            this.Hide();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            QuanLy detect = new QuanLy();
            detect.Show();
            this.Hide();
        }
       
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult h = MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Thông báo", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
            {
                DangNhap a = new DangNhap();
                a.Show();
                this.Hide();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            BaoCao report = new BaoCao();
            report.Show();
            this.Hide();
        }

        private void btnDiemDanhThuCong_Click(object sender, EventArgs e)
        {
            FormDiemDanhThuCong ddtc = new FormDiemDanhThuCong();
            ddtc.Show();
            this.Hide();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
