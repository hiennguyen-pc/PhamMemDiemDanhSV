using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiemDanhBangKhuonMat
{
    public partial class FormDiemDanhThuCong : Form
    {
        Xuly dt = new Xuly();
        public FormDiemDanhThuCong()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormDiemDanhThuCong_Load(object sender, EventArgs e)
        {
            cbbKhoa.DataSource = dt.loadKhoa();
            cbbKhoa.DisplayMember = "Tenkhoa";
            cbbKhoa.ValueMember = "Makhoa";
            cbbKhoa.BindingContext = this.BindingContext;
            cbbLop.DataSource = dt.loadLop2(cbbKhoa);
            cbbLop.DisplayMember = "Tenlop";
            cbbLop.ValueMember = "Malop";
            
           
        }

        private void cbbKhoa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbbLop.DataSource = dt.loadLop2(cbbKhoa);
            cbbLop.DisplayMember = "Tenlop";
            cbbLop.ValueMember = "Malop";
        }

        private void cbbLop_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dt.loadStudent(cb_mon);
            dataGridView1.DataSource = dt.qlSinhVien.Tables["loadDIEMDANH"];
        }

        private void BackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void CloseProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnDiemDanhThuCong_Click(object sender, EventArgs e)
        {
            if (dt.thucHienDiemDanh(dataGridView1,cb_mon.SelectedValue.ToString()))
            {
                MessageBox.Show("Điểm Danh Thành Công!");
            }
            else
            {
                MessageBox.Show("Điểm danh thất bại");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1;i++ )
            {
                dataGridView1.Rows.RemoveAt(i);
            }
                
            dataGridView1.DataSource = dt.TimKiem(txtTimKiem);
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
         //dataGridView1.DataSource = null;
         //dataGridView1.Refresh();

         //  dataGridView1.DataSource = dt.TimKiem(txtTimKiem);
        }

        private void cbbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_mon.DataSource = dt.loadMonHoc(cbbLop.SelectedValue.ToString());
            cb_mon.DisplayMember = "TENMH";
            cb_mon.ValueMember = "MAMH";
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cb_mon_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt.loadStudent(cb_mon);
            dataGridView1.Refresh();
            dataGridView1.DataSource = dt.qlSinhVien.Tables["loadDIEMDANH"];
        }


    }
}
