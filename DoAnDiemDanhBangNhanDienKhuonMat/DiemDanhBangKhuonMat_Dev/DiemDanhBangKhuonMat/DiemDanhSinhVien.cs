using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Data.SqlClient;

namespace DiemDanhBangKhuonMat
{
    public partial class DiemDanhSinhVien : Form
    {
        //initializing 
        Xuly xuly = new Xuly();
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        Image<Gray, byte> result, TrainedFace = null, TrainedEyes = null, TrainedMouth = null, TrainedNose = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        //Initializing a list to save detected names of students
        List<string> NamePersons = new List<string>();
        string name = null, names = null;
        int t, ContTrain, NumLabels;

        SqlConnection con; //connection

        private HashSet<string> FacesAlreadyDetected = new HashSet<string>();

        private void resetAttendanceButton_Click(object sender, EventArgs e)
        {
            FacesAlreadyDetected.Clear();
        }
        public void fun(Label txtForm1)
        {
            label1.Text = txtForm1.Text;
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //var txtpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionString.txt");
            //StreamReader sr = new StreamReader(txtpath);
            //String line = sr.ReadToEnd();
            //con = new SqlConnection(@""+ line + "");
            //SqlDataAdapter checkup = new SqlDataAdapter("SELECT * FROM attendance", con); //this will get all marked attendance from the database
            //DataTable sd = new DataTable();

            //checkup.Fill(sd);
            //dataGridView1.DataSource = sd;

            //DataTable sd1 = new DataTable();
            //sd1 = sd.DefaultView.ToTable(true, "name", "studentid", "dateandtime","Makhoa","Malop");

            dataGridView1.DataSource = xuly.load_tbDD() ;
        }

        private void closeProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grabber = new Capture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber);
        }
        
        public DiemDanhSinhVien()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade-frontalface-default.xml");
            try
            {
                //Load previous trainned faces of students and their names
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedNames.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/faces/" + LoadFaces));
                    //make a list of string
                    labels.Add(Labels[tf]); 
                }  
            }
            catch (Exception e)
            {
                //MessageBox.Show("Press OK to proceed!");
            }
        }

        public void FrameGrabber(object sender, EventArgs e)
        {
            xuly.loadDD();
            NamePersons.Add("");
            //Detect number of faces on screen
            //label5.Text = "0";

            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(501, 407, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            //Camera IP
            //currentFrame = new Emgu.CV.CvInvoke.cvCreateFileCapture("http://username:pass@cam_address/axis-cgi/mjpg/video.cgi?resolution=640x480&req_fps=30&.mjpg");

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
          MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.3,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);
                //initialize result,t and gray if (trainingImages.ToArray().Length != 0)
                {
                    //term criteria against each image to find a match with it, perform different iterations
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                    //call class by creating object and pass parameters
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                         trainingImages.ToArray(),
                         labels.ToArray(),
                         5000,
                         ref termCrit);
                    //next step is to name find for recognize face
                    name = recognizer.Recognize(result);
                    //now show recognized person name so
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));//initalize font for the name captured

                }

                if (!FacesAlreadyDetected.Contains(name))
                {
                    
                    //SaveToDatabase(name, DateTime.Now);
                    FacesAlreadyDetected.Add(name);
                    xuly.loadStudent(cb_monhoc);
                    if (xuly.inLoadDD(name))
                    {

                        
                        xuly.ThemDD(name, cb_monhoc.SelectedValue.ToString(), DateTime.Now);
                        dataGridView1.DataSource = xuly.load_tbDD();
                    }
                  

                }
                

                NamePersons[t - 1] = name;
                NamePersons.Add("");
                //check detected faces 
                //label5.Text = facesDetected[0].Length.ToString();
            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
                //MessageBox.Show(NamePersons[nnn]);

                string test = NamePersons[nnn] + ",";

                System.IO.File.AppendAllText(Application.StartupPath + "/Names/names.txt", test);

            }
            //load haarclassifier and previous saved images to find matches
            imageBox1.Image = currentFrame;
            //label3.Text = names;
            names = "";
            NamePersons.Clear();


        }

        
        private void DetectAndAttendance_Load(object sender, EventArgs e)
        {
            cb_khoa.DataSource = xuly.loadKhoa();
            cb_khoa.DisplayMember = "Tenkhoa";
            cb_khoa.ValueMember = "Makhoa";
            cb_lop.DataSource = xuly.loadLop(cb_khoa.SelectedValue.ToString());
            cb_lop.DisplayMember = "Tenlop";
            cb_lop.ValueMember = "Malop";
        }

        private void cb_lop_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_monhoc.DataSource = xuly.loadMonHoc(cb_lop.SelectedValue.ToString());
            cb_monhoc.DisplayMember = "TENMH";
            cb_monhoc.ValueMember = "MAMH";
        }

        private void cb_khoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_lop.DataSource = xuly.loadLop(cb_khoa.SelectedValue.ToString());
            cb_lop.DisplayMember = "Tenlop";
            cb_lop.ValueMember = "Malop";
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cb_monhoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
