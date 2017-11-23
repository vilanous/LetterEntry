using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;
using System.Data;
using System.IO;
using LetterEntry;
using TwainDotNet.Wpf;
using TwainDotNet;
using TestAppWpf;
using System.Net.Mail;
using System.Threading;
using System.Configuration;

namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        public String getdoctype, getSenderName;
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30";

        //public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30");


        public MainWindow()
        {
            InitializeComponent();
            OnLogin();
            FillComboClass();
            FillIDbox();

            comboDocType.Focus();
            AppWindow = this;
            
        }
        public void OnLogin()
        {
            string CurrentUser = null;
            string CurrentUserAccess = null;
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            dBConn.Open();
            SqlCommand GetUser = new SqlCommand("SELECT USERNAME,ACCESS_LEVEL FROM SESSION", dBConn);
            SqlDataReader UserDataReader;
            try
            {
                UserDataReader = GetUser.ExecuteReader();
                while (UserDataReader.Read())
                {
                    CurrentUser = UserDataReader["USERNAME"].ToString();
                    CurrentUserAccess = UserDataReader["ACCESS_LEVEL"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (CurrentUserAccess != "Administrator")
            {
                Admin_Btn.IsEnabled = false;
                
            }
        }
        public void Login_Window()
        {
            LoginPage NewLogin = new LoginPage();
            NewLogin.ShowDialog();
        }
        public void FillIDbox()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            dBConn.Open();
            string IdPlusOne;
            int IdFromTbl;
            SqlCommand GetID = new SqlCommand("SELECT ID FROM tableEntry", dBConn);
            
            SqlDataReader GetIDReader;
            try
            {
                GetIDReader = GetID.ExecuteReader();
                while(GetIDReader.Read())
                {
                    IdFromTbl = GetIDReader.GetInt32(0);
                    IdPlusOne = Convert.ToString( IdFromTbl + 1 );
                    IDBox.Text = "00" + IdPlusOne;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
            dBConn.Dispose();
        }
        public void FillComboClass()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            comboDocType.Items.Clear();
            sentByBox.Items.Clear();
            string getdoctype, getSenderName;
            string Query1 = "SELECT docType FROM tableDocumentType";
            //SqlConnection dBConn = new SqlConnection(dBPath);
            SqlCommand getdoc = new SqlCommand(Query1, dBConn);

            string Query2 = "SELECT Contact_Name FROM tableSenderList";
            SqlCommand getSender = new SqlCommand(Query2, dBConn);

            SqlDataReader myreader1;
            SqlDataReader myreader2;
            try
            {
                dBConn.Open();
                myreader1 = getdoc.ExecuteReader();
                while (myreader1.Read())
                {
                    getdoctype = myreader1.GetString(0);
                    comboDocType.Items.Add(getdoctype);
                }
                myreader1.Close();
                myreader2 = getSender.ExecuteReader();
                while (myreader2.Read())
                {
                    getSenderName = myreader2.GetString(0);
                    sentByBox.Items.Add(getSenderName);
                }
                myreader2.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
        
        private void Button_Document_Type(object sender, RoutedEventArgs e)
        {
            DocumentType newDocType = new DocumentType();
            newDocType.ShowDialog();
        }

        private void Button_Add_Sender(object sender, RoutedEventArgs e)
        {
            SenderList NewAddSender = new SenderList();
            NewAddSender.ShowDialog();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

            //dBConn.Open();

            //try
            //{
                SaveEntry();
                SendMail();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            Reset_Form();
            FillComboClass();
            FillIDbox();

        }
        public void SaveEntry()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            SqlCommand save = new SqlCommand();
            dBConn.Open();
            string thedoctype = comboDocType.Text;
            string thedate = currentDate.SelectedDate.ToString();
            string theref = docRef.Text;
            string thedcrp = drcpbox.Text;
            string thefrom = sentByBox.Text;
            string sAssignedDepart = sAssingedDepartCombo.Text;

            save.CommandText = "INSERT INTO tableEntry (Doc_Type,Doc_Number,Sent_By,Details,Entry,Date)" +
                " VALUES(@Doc_Type,@Doc_Number,@Sent_By,@Details,@Entry,@Date)";
            save.Parameters.AddWithValue("@Doc_Type", thedoctype);
            save.Parameters.AddWithValue("@Doc_Number", theref);
            save.Parameters.AddWithValue("@Sent_By", thefrom);
            save.Parameters.AddWithValue("@Details", thedcrp);
            save.Parameters.AddWithValue("@Entry", sAssignedDepart);
            save.Parameters.AddWithValue("@Date", thedate);
            save.Connection = dBConn;
            save.ExecuteNonQuery();

            //MessageBox.Show("Successfull");
            
            dBConn.Close();
            dBConn.Dispose();
            MessageBox.Show("Successfull");
            
        }
        public void SendMail()
        {
            String userName = "majid.ali@mtcc.com.mv";
            String password = "gis@1245";
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(userName);
            msg.To.Add("vilanous@gmail.com");
            msg.Subject = "Your Subject Name";
            string sb = "test string for mail";
            //sb.AppendLine("Name: " + txtname.Text);
            //sb.AppendLine("Mobile Number: " + txtmbno.Text);
            //sb.AppendLine("Email:" + txtemail.Text);
            //sb.AppendLine("Drop Downlist Name:" + ddllinksource.SelectedValue.ToString());
            msg.Body = sb;
            //Attachment attach = new Attachment(Server.MapPath("folder/" + ImgName));
            //msg.Attachments.Add(attach);
            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Credentials = new System.Net.NetworkCredential(userName, password);
            SmtpClient.Host = "smtp.office365.com";
            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.Send(msg);
            MessageBox.Show("sent");
        }
        

        private void AdminBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel NewAdminPanel = new AdminPanel();
            NewAdminPanel.ShowDialog();
        }

        private void UsrMngmt_Click(object sender, RoutedEventArgs e)
        {
            UserManagement NewUserMngmt = new UserManagement();
            NewUserMngmt.ShowDialog();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            Reset_Form();
        }

        private void ScanBtn_Click(object sender, RoutedEventArgs e)
        {
            Window1 NewScanner = new Window1();
            NewScanner.ShowDialog();
        }

        
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Login_Window();
            Close();
            
        }

        private void ReportsBtn_Click(object sender, RoutedEventArgs e)
        {
            Reports NewReport = new Reports();
            NewReport.ShowDialog();
        }

        public void Reset_Form()
        {
            comboDocType.Text = "";
            docRef.Text = "";
            drcpbox.Text = "";
            sentByBox.Text = "";
            sAssingedDepartCombo.Text = "";

            comboDocType.Focus();
        }
    }

}