using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace DiemDanhBangKhuonMat
{
    public partial class BaoCao : Form
    {
        public SqlConnection cn;
        SqlConnection con;
        public BaoCao()
        {
            InitializeComponent();
        }
        Xuly xuly = new Xuly();
        
        private void BaoCao_Load(object sender, EventArgs e)
        {
           
            

            var txtpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionString.txt");
            StreamReader sr = new StreamReader(txtpath);
            String line = sr.ReadToEnd();
            con = new SqlConnection(@"" + line + "");
            SqlDataAdapter checkup = new SqlDataAdapter("select SINHVIEN.MASV,TENSV,NGAYDD from DIEMDANH,SINHVIEN where SINHVIEN.MASV=DIEMDANH.MASV", con); //this will get all marked attendance from the database
            DataTable sd = new DataTable();

            checkup.Fill(sd);
            dataGridView1.DataSource = sd;

            DataTable sd1 = new DataTable();
            sd1 = sd.DefaultView.ToTable(true, "MASV", "TENSV", "NGAYDD");

            dataGridView1.DataSource = sd1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void trởVềToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

       

       

        private void button2_Click(object sender, EventArgs e)
        {
            string theDate = DateTime.Now.ToLongDateString();
                try
                {
                    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                    app.Visible = true;
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet = workbook.ActiveSheet;
                    worksheet.Name = theDate ;

                    try
                    {
                       
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                        }
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                if (dataGridView1.Rows[i].Cells[j].Value != null)
                                {
                                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                                }
                                else
                                {
                                    worksheet.Cells[i + 2, j + 1] = "";
                                }
                            }
                        }

                        //Getting the location and file name of the excel to save from user. 
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveDialog.FilterIndex = 2;

                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            workbook.SaveAs(saveDialog.FileName);
                            MessageBox.Show("Xuất báo cáo thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            xuly.xoaDD();
                            Main main = new Main();
                            main.Show();
                            this.Hide();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Lỗi : " + ex.Message);
                    }

                    finally
                    {
                        app.Quit();
                        workbook = null;
                        worksheet = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message.ToString());
                }
        }

    }
}
