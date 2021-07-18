using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Forms;

namespace DiemDanhBangKhuonMat
{
    class Xuly
    {
        SqlConnection cnn = new SqlConnection("Data Source=MSI;Initial Catalog=QL_DIEMDANH;Persist Security Info=True;User ID=sa;Password=123");
        public DataSet qlSinhVien = new DataSet();
        SqlDataAdapter da_dd;
        SqlCommand cmd;
        SqlDataAdapter dsSV;
        SqlDataAdapter sv_train;
        public Xuly()
        {
            loadDD();
            loadSVtrain();
            SqlDataAdapter da_tk = new SqlDataAdapter("Select *from TAIKHOAN", cnn);
            da_tk.Fill(qlSinhVien, "TAIKHOAN");

        }
        public bool Login(string user, string pass){
             try
            {
                var txtpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionString.txt");
                StreamReader sr = new StreamReader(txtpath);
                String cs = sr.ReadToEnd();

                SqlConnection myConnection = default(SqlConnection);
                myConnection = new SqlConnection(@"" + cs + "");

                SqlCommand myCommand = default(SqlCommand);

                myCommand = new SqlCommand("SELECT TENDANGNHAP,PASS FROM taikhoan WHERE TENDANGNHAP = @Username AND PASS = @Password", cnn);

                SqlParameter uName = new SqlParameter("@Username", SqlDbType.VarChar);
                SqlParameter uPassword = new SqlParameter("@Password", SqlDbType.VarChar);

                uName.Value = user;
                uPassword.Value = pass;

                myCommand.Parameters.Add(uName);
                myCommand.Parameters.Add(uPassword);

                myCommand.Connection.Open();

                SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (myReader.Read())
                {
                    if (myConnection.State == ConnectionState.Open)
                    {
                        myConnection.Dispose();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
              
               
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public DataTable loadKhoa()
        {
            SqlDataAdapter da_khoa = new SqlDataAdapter("Select * From Khoa", cnn);
            da_khoa.Fill(qlSinhVien, "khoa");
            return qlSinhVien.Tables["khoa"];
        }
        public DataTable loadLop(string maKhoa)
        {
            SqlDataAdapter da_lop = new SqlDataAdapter("Select * From Lop Where Makhoa = '" + maKhoa + "'", cnn);
            if (qlSinhVien.Tables["lop"] != null) {
                qlSinhVien.Tables.Remove("lop");
            }
            da_lop.Fill(qlSinhVien, "lop");
            return qlSinhVien.Tables["lop"];
        }
        public void loadSV()
        {
            dsSV = new SqlDataAdapter("select *from SINHVIEN",cnn);
            dsSV.Fill(qlSinhVien, "SINHVIEN");
            DataColumn[] khoachinh = new DataColumn[1];
            khoachinh[0] = qlSinhVien.Tables["SINHVIEN"].Columns[0];
            qlSinhVien.Tables["SINHVIEN"].PrimaryKey = khoachinh;
            
        }
        public bool inserDatabase(string ten, string masv,string malop)
        {
            try
            {
                DataRow dr_them = qlSinhVien.Tables["SINHVIEN"].NewRow();
                dr_them["TENSV"] = ten;
                dr_them["MASV"] = masv;
                dr_them["MALOP"] = malop;
                qlSinhVien.Tables["SINHVIEN"].Rows.Add(dr_them);
                SqlCommandBuilder build = new SqlCommandBuilder(dsSV);
                dsSV.Update(qlSinhVien, "SINHVIEN");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool themsv(string masv)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(@"INSERT INTO SVTRAIN ([MASV]) VALUES @MASV);", cnn);
                
                cmd.Parameters.AddWithValue("@MASV", masv);
                
                int i = cmd.ExecuteNonQuery();
                cnn.Close();

                if (i == 1)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        public DataTable loadLop2(ComboBox cbb)
        {
            SqlDataAdapter da_lop = new SqlDataAdapter("Select * From Lop Where Makhoa='" + cbb.SelectedValue + "'", cnn);
            DataTable dt = new DataTable();
            da_lop.Fill(dt);
            return dt;
        }
        public DataTable loadDiemDanh()
        {
            SqlDataAdapter da_dd = new SqlDataAdapter("SELECT * FROM DIEMDANH", cnn);
            da_dd.Fill(qlSinhVien, "DIEMDANH");
            DataColumn[] khoachinh = new DataColumn[1];
            khoachinh[0] = qlSinhVien.Tables["DIEMDANH"].Columns[0];
            qlSinhVien.Tables["DIEMDANH"].PrimaryKey = khoachinh;
            return qlSinhVien.Tables["DIEMDANH"];
        }
        public DataTable loadQL(string malop)
        {
            if (qlSinhVien.Tables["quanly"] != null)
            {
                qlSinhVien.Tables.Remove("quanly");
            }
            SqlDataAdapter da_ql = new SqlDataAdapter("	select MASV,TENSV,Khoa.Tenkhoa,Lop.Tenlop from SINHVIEN,Khoa,Lop where SINHVIEN.Malop=Lop.Malop and LOP.MAKHOA=KHOA.MAKHOA and lop.MALOP='"+malop+"'", cnn);
            da_ql.Fill(qlSinhVien,"quanly");
            return qlSinhVien.Tables["quanly"];

        }
        public bool xoa(string id)
        {
            try
            {
                DataRow dr_xoa = qlSinhVien.Tables["SINHVIEN"].Rows.Find(id);
                if (dr_xoa != null)
                {
                    dr_xoa.Delete();
                    SqlCommandBuilder build = new SqlCommandBuilder(dsSV);
                    dsSV.Update(qlSinhVien, "SINHVIEN");
                    return true;
                }
                return false;
                
            }
            catch{
                return false;
            }
        }

        //
        public void loadStudent(ComboBox cbb)
        {
            if (qlSinhVien.Tables["loadDIEMDANH"] != null)
            {
                qlSinhVien.Tables.Remove("loadDIEMDANH");
            }
            SqlDataAdapter da_loadDD = new SqlDataAdapter("	select MASV,TENSV,SINHVIEN.MALOP,TENMH  from LOPHOCPHAN,MONHOC,LOP,SINHVIEN where LOPHOCPHAN.MALOP=LOP.MALOP and LOPHOCPHAN.MAMH=MONHOC.MAMH and SINHVIEN.MALOP=LOP.MALOP and MONHOC.MAMH='"+cbb.SelectedValue.ToString()+"'", cnn);
            da_loadDD.Fill(qlSinhVien, "loadDIEMDANH");
            DataColumn[] khoachinh = new DataColumn[1];
            khoachinh[0] = qlSinhVien.Tables["loadDIEMDANH"].Columns[0];
            qlSinhVien.Tables["loadDIEMDANH"].PrimaryKey = khoachinh;
            
        }
        public bool inLoadDD(string masv)
        {
            DataRow dr_in = qlSinhVien.Tables["loadDIEMDANH"].Rows.Find(masv);
            if (dr_in != null)
            {
                return true;
            }
            return false;
        }

        // bắt đầu làm điểm danh.
        public void loadDD()
        {
            da_dd = new SqlDataAdapter("Select * from DIEMDANH", cnn);
            da_dd.Fill(qlSinhVien, "DIEMDANH");
            DataColumn[] khoachinh = new DataColumn[1];
            khoachinh[0] = qlSinhVien.Tables["DIEMDANH"].Columns[0];
            qlSinhVien.Tables["DIEMDANH"].PrimaryKey = khoachinh;
            //return qlSinhVien.Tables["DIEMDANH"];
        }
        public bool ThemDD(string masv, string mamh, DateTime date)
        {
            try
            {
                DataRow dr_them = qlSinhVien.Tables["DIEMDANH"].NewRow();
                dr_them["MASV"] = masv;
                dr_them["MAMH"] = mamh;
                dr_them["NGAYDD"] = date;
                qlSinhVien.Tables["DIEMDANH"].Rows.Add(dr_them);
                SqlCommandBuilder build =new SqlCommandBuilder(da_dd);
                da_dd.Update(qlSinhVien, "DIEMDANH");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void loadSVtrain()
        {
            sv_train = new SqlDataAdapter("select *from SVTRAIN", cnn);
            sv_train.Fill(qlSinhVien, "SVTRAIN");
            DataColumn[] kc = new DataColumn[1];
            kc[0] = qlSinhVien.Tables["SVTRAIN"].Columns[0];
            qlSinhVien.Tables["SVTRAIN"].PrimaryKey = kc;
        }
        public bool themTrain(string masv)
        {
            try
            {
                DataRow dr_them = qlSinhVien.Tables["SVTRAIN"].NewRow();
                dr_them["MASV"] = masv;
                qlSinhVien.Tables["SVTRAIN"].Rows.Add(dr_them);
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool luu()
        {
            try
            {
                SqlCommandBuilder build = new SqlCommandBuilder(sv_train);
                sv_train.Update(qlSinhVien, "SVTRAIN");
                return true;
            }
            catch
            {
                return false;
            }
        }
        //public bool thucHienDiemDanh(DataGridView dtg)
        //{
        //    try
        //    {
        //        for (int i = 0; i < dtg.Rows.Count - 1; i++)
        //        {
        //            DataRow dr_them = qlSinhVien.Tables["attendance"].NewRow();
        //            dr_them["id"] = i + 1;
        //            dr_them["studentid"] = dtg.Rows[i].Cells[1].Value;
        //            dr_them["name"] = dtg.Rows[i].Cells[2].Value;
        //            dr_them["dateandtime"] = DateTime.Now; //dtg.Rows[1].Cells[3].Value;
        //            dr_them["Makhoa"] = dtg.Rows[i].Cells[4].Value;
        //            dr_them["Malop"] = dtg.Rows[i].Cells[3].Value;
        //            qlSinhVien.Tables["attendance"].Rows.Add(dr_them);
        //            SqlCommandBuilder build = new SqlCommandBuilder(da_dd);
        //            da_dd.Update(qlSinhVien, "attendance");
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public bool thucHienDiemDanh(DataGridView dtg,string monhoc)
        {
            try
            {
                for (int i = 0; i < dtg.Rows.Count - 1; i++)
                {
                    if (dtg.Rows[i].Selected)
                    {
                        DataRow dr_them = qlSinhVien.Tables["DIEMDANH"].NewRow();
                        dr_them["MASV"] = dtg.Rows[i].Cells[0].Value;
                        dr_them["MAMH"] = monhoc;
                        dr_them["NGAYDD"] = DateTime.Now;
                        qlSinhVien.Tables["DIEMDANH"].Rows.Add(dr_them);
                        SqlCommandBuilder build = new SqlCommandBuilder(da_dd);
                        da_dd.Update(qlSinhVien, "DIEMDANH");
                        dtg.Rows.RemoveAt(i);   
                    }
                    
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public DataTable TimKiem(TextBox mssv)
        {
            SqlDataAdapter da_timkiem = new SqlDataAdapter("select studentid,name,Makhoa,Malop from student where name like '%" + mssv.Text + "%' or studentid like '%" + mssv.Text + "%' or Malop like '%" + mssv.Text + "%' or Makhoa like '%" + mssv.Text + "%'", cnn);
            da_timkiem.Fill(qlSinhVien, "TimKiem");
            return qlSinhVien.Tables["TimKiem"];
        }
       // string sql = "Select * from KHACHHANG where MAKH LIKE '%" + txt_TK.Text + "%' or  HOTEN LIKE '%" + txt_TK.Text + "%'";
        public DataTable loadsv(string malop)
        {
            if (qlSinhVien.Tables["train"] != null)
            {
                qlSinhVien.Tables.Remove("train");
            }
            SqlDataAdapter da_sv = new SqlDataAdapter("Select MASV from SINHVIEN where MALOP='" + malop + "'", cnn);
            da_sv.Fill(qlSinhVien, "train");
            return qlSinhVien.Tables["train"];
        }
        //LOad môn học
        public DataTable loadMonHoc(string malop)
        {
            if (qlSinhVien.Tables["MONHOC"] != null)
            {
                qlSinhVien.Tables.Remove("MONHOC");
            }
            SqlDataAdapter da_mh = new SqlDataAdapter("select LOPHOCPHAN.MAMH,TENMH FROM LOPHOCPHAN,MONHOC,LOP WHERE LOPHOCPHAN.MAMH=MONHOC.MAMH AND LOPHOCPHAN.MALOP=LOP.MALOP AND LOPHOCPHAN.MALOP='"+malop+"'", cnn);
            da_mh.Fill(qlSinhVien, "MONHOC");
            return qlSinhVien.Tables["MONHOC"];
        }
        //Load Bảng điểm danh
        public DataTable load_tbDD()
        {
            SqlDataAdapter da_load = new SqlDataAdapter("select MASV,TENMH,NGAYDD from DIEMDANH,MONHOC where DIEMDANH.MAMH=MONHOC.MAMH", cnn);
            DataTable dt = new DataTable();
            da_load.Fill(dt);
            return dt;
           
        }
        public void xoaDD()
        {
            SqlDataAdapter xoaDD = new SqlDataAdapter("DELETE from DIEMDANH", cnn);
            xoaDD.Fill(qlSinhVien.Tables["DIEMDANH"]);
            
        }
        
    }
    

}
