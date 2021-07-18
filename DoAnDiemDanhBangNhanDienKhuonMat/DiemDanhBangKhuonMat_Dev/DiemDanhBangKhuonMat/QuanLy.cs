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
using System.IO;
namespace DiemDanhBangKhuonMat
{
    public partial class QuanLy : Form
    {
        public QuanLy()
        {
            InitializeComponent();
        }
        Xuly xuly = new Xuly();
        public SqlConnection cn;
        string id = "";
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        DataTable tb;
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        
        public void ketnoi()
        {
            var txtpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionString.txt");
            StreamReader sr = new StreamReader(txtpath);
            String line = sr.ReadToEnd();
            cn = new SqlConnection(@""+line+"");
            cn.Open();
        }
        private void LoadCobkhoa()
        {
            DataTable dt = new DataTable();
            var txtpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionString.txt");
            StreamReader sr = new StreamReader(txtpath);
            String line = sr.ReadToEnd();
            cn = new SqlConnection(@"" + line + "");
            //cn = new SqlConnection("Server=DESKTOP-MHU5E4L;Initial Catalog=FRSYSTEM_DATABASE;Integrated Security=True");
            cn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From Khoa ", cn);

                da.Fill(dt);
                cb_khoa.DataSource = dt;
                cn.Close();
            }
            catch (Exception ex)
            {
                // throw new Exception("Error " + ex.ToString());
            }

            try
            {
                cb_khoa.DataSource = dt;
                cb_khoa.DisplayMember = "Tenkhoa";
                //comboBox1.ValueMember = "Tenkhoa";
                cb_khoa.ValueMember = "Makhoa";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi load dữ liệu!\n", ex.ToString());
            }
        }
        private void LoadCoblop()
        {
            DataTable dt = new DataTable();
            
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * From Lop Where Makhoa = '" + cb_khoa.SelectedValue + "'", cn);

                da.Fill(dt);
                cb_lop.DataSource = dt;
                cn.Close();
            }
            catch (Exception ex)
            {
                // throw new Exception("Error " + ex.ToString());
            }

            try
            {
                cb_lop.DataSource = dt;
                cb_khoa.DisplayMember = "Tenlop";
                cb_lop.ValueMember = "Malop";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi load dữ liệu!\n", ex.ToString());
            }
        }

        protected void Load_Data()
        {

            SqlDataAdapter da = new SqlDataAdapter("select name, studentid,Khoa.Tenkhoa,Lop.Tenlop from STUDENT,Khoa,Lop where STUDENT.Malop=Lop.Malop and STUDENT.Makhoa=Khoa.Makhoa", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
           
            dataGridView1.DataSource = dt;
           
            
            txtTen.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["name"].Value);
            txtMa.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["studentid"].Value);
            cb_khoa.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["Tenkhoa"].Value);
            cb_lop.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["TenLop"].Value);
        }
        private void QuanLy_Load(object sender, EventArgs e)
        {
            try
            {
                ketnoi();
                //MessageBox.Show("Thành công");
            }
            catch (Exception loi)
            {
                //MessageBox.Show("Thất bại");
            }


            cb_khoa.DataSource = xuly.loadKhoa();
            cb_khoa.DisplayMember = "TenKhoa";
            cb_khoa.ValueMember = "MaKhoa";
            cb_lop.DataSource = xuly.loadLop(cb_khoa.SelectedValue.ToString());
            cb_lop.DisplayMember = "TENLOP";
            cb_lop.ValueMember = "MALOP";

            cn.Open();

            //Load_Data();
            xuly.loadSV();
            
           

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataGridView1.CurrentRow.Index;
            txtMa.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["MASV"].Value);
            txtTen.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["TENSV"].Value);
            
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            
            xuly.loadSV();
            if (xuly.xoa((string)dataGridView1.CurrentRow.Cells["MASV"].Value))
            {
                
                MessageBox.Show("Xóa Thành công");
                dataGridView1.DataSource = xuly.loadQL(cb_lop.SelectedValue.ToString());
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string id = (string)dataGridView1.CurrentRow.Cells["MASV"].Value;
            string sql_up = "Update SINHVIEN set TENSV= N'"+txtTen.Text+"' where MASV = '"+id+"'";
            SqlCommand cmd = new SqlCommand(sql_up, cn);
            cn.Close();
            cn.Open();
            int kq = cmd.ExecuteNonQuery();
            if (kq > 0)
            {
                MessageBox.Show("Update thành công!");
                dataGridView1.DataSource = xuly.loadQL(cb_lop.SelectedValue.ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCoblop();
        }

        private void đóngChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void trởVềToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DialogResult h = MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Thông báo", MessageBoxButtons.OKCancel);
            //if (h == DialogResult.OK)
            //{
            //    DangNhap login = new DangNhap();
            //    login.Show();
            //    this.Close();
            //}
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void cb_lop_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = xuly.loadQL(cb_lop.SelectedValue.ToString());
            txtMa.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["MASV"].Value);
            txtTen.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["TENSV"].Value);

        }
    }
}
