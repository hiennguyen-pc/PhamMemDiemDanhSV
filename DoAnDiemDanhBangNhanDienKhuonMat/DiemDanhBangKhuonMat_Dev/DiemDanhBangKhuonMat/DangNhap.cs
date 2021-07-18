using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;


namespace DiemDanhBangKhuonMat
{
    public partial class DangNhap : Form
    {

        Xuly xuly = new Xuly();
        public DangNhap()
        {
            InitializeComponent();
        }
        public delegate void delPassData(TextBox text);
        
        private void button2_Click(object sender, EventArgs e)
        {
            txtUserName.ResetText();
            txtPassword.ResetText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
            if (xuly.Login(txtUserName.Text, txtPassword.Text))
            {
                Main main = new Main();
                main.Show();
                this.Hide();
                delPassData del = new delPassData(main.funData);
                del(this.txtUserName);
                main.Show();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra tên tài khoản và mật khẩu !", "Đăng nhập bị từ chối !", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtUserName.Clear();
                txtPassword.Clear();
                txtUserName.Focus();
            }
            
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {
            button1.Focus();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://sinhvien.hufi.edu.vn/");
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Process.Start("http://gmail.com/");
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult h = MessageBox.Show ("Bạn có chắc muốn thoát khỏi phần mềm không?", "Thông báo", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
                Application.Exit();
        }

    }
}
