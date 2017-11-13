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
        public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30");

        public MainWindow()
        {
            InitializeComponent();
            FillComboClass();
            FillIDbox();

            comboDocType.Focus();
            AppWindow = this;
        }

        public void Login_Window()
        {
            LoginPage NewLogin = new LoginPage();
            NewLogin.ShowDialog();
        }
        public void FillIDbox()
        {
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
                    IDBox.Text = IdPlusOne;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
        public void FillComboClass()
        {   
            
            comboDocType.Items.Clear();
            sentByBox.Items.Clear();
            string getdoctype, getSenderName;
            string Query1 = "SELECT docType FROM tableDocumentType";
            SqlConnection dBConn = new SqlConnection(dBPath);
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
            SqlCommand save = new SqlCommand();
            dBConn.Open();
            string thedoctype = comboDocType.Text;
            string thedate = currentDate.SelectedDate.ToString();
            string theref = docRef.Text;
            string thedcrp = drcpbox.Text;
            string thefrom = sentByBox.Text;
            string theenterer = entryBybox.Text;

            save.CommandText = "INSERT INTO tableEntry (Doc_Type,Doc_Number,Sent_By,Details,Entry,Date) VALUES(@Doc_Type,@Doc_Number,@Sent_By,@Details,@Entry,@Date)";
            save.Parameters.AddWithValue("@Doc_Type", thedoctype);
            save.Parameters.AddWithValue("@Doc_Number", theref);
            save.Parameters.AddWithValue("@Sent_By", thefrom);
            save.Parameters.AddWithValue("@Details", thedcrp);
            save.Parameters.AddWithValue("@Entry", theenterer);
            save.Parameters.AddWithValue("@Date", thedate);
            save.Connection = dBConn;
            save.ExecuteNonQuery();
            
            //MessageBox.Show("Successfull");
            dBConn.Close();

            //clear all
            Reset_Form();
            MessageBox.Show("Successfull");
            //call fill datagrid function
            //Fill_Data_Grid_Class();
            FillComboClass();
            FillIDbox();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("Admin@gis.edu.mv");
                mail.To.Add("majid.ali@mtcc.com.mv");
                mail.Subject = "GIS Entry System, ENTRY ID: " + IDBox.Text;
                mail.Body = thefrom + "\n" + thedate + "\n" + thedoctype + "\n" + theref + "\n" + thedcrp;

                //Attachment attachment;
                //attachment = new Attachment("your attachment file");
                //mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("debugservice0@gmail.com", "manage0service");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                //MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            //Reset_Form();


        }

        private void AdminBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel NewAdminPanel = new AdminPanel();
            NewAdminPanel.ShowDialog();
        }

        private void UsrMngmt_Click(object sender, RoutedEventArgs e)
        {

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
            this.Hide();
            Login_Window();
            this.Close();
            
        }

        private void ReportsBtn_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        public void Reset_Form()
        {
            comboDocType.Text = "";
            docRef.Text = "";
            drcpbox.Text = "";
            sentByBox.Text = "";
            entryBybox.Text = "";

            comboDocType.Focus();
        }
    }

}
