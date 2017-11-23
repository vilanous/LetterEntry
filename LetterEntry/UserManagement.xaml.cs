using System;
using System.Linq;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections;
using System.Configuration;

namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Window
    {
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30";
        //public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30");
        public UserManagement()
        {
            InitializeComponent();
            //Fill_Data_Grid_Class();
            Fill_sNameCombo();
            Fill_sAccessCombo();
            Fill_sStatusCombo();
        }

        public void Fill_Data_Grid_Class()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            string CmdString = string.Empty;
            //SqlConnection dBConn = new SqlConnection(dBPath);
            CmdString = "SELECT STAFF_ID as 'ID', STAFF_NAME as 'NAME', USERNAME as 'USERNAME', EMAIL as 'EMAIL', DESIGNATION as 'DESIGNATION'" +
                ", ACCESS_LEVEL as 'ACCESS LEVEL', DEPART_ALIAS as 'DEPARTMENT', STATUS as 'STATUS' FROM STAFF";
            SqlCommand cmd = new SqlCommand(CmdString, dBConn);
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("STAFF");
            ada.Fill(dt);
            AllUsersGrid.ItemsSource = dt.DefaultView;

        }
        public void Fill_Sessions()
        {
            try
            {
                SqlConnection dBConn = new SqlConnection();
                dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
                SqlCommand FillSession_Cmd = new SqlCommand("SELECT SESSION_TIME,USERNAME,ACCESS_LEVEL FROM SESSION", dBConn);
                SqlDataAdapter SessionsAdapter = new SqlDataAdapter(FillSession_Cmd);
                DataTable SessionTable = new DataTable("SESSION");
                SessionsAdapter.Fill(SessionTable);
                SessionGrid.ItemsSource = SessionTable.DefaultView;
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            
        }
        public void Fill_sNameCombo()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            sNameCombo.Items.Clear();
            string sNameFromtbl;
            string Query1 = "SELECT STAFF_NAME FROM STAFF";
            SqlCommand GetStaff = new SqlCommand(Query1, dBConn);

            SqlDataReader myreader1;
            try
            {
                dBConn.Open();
                myreader1 = GetStaff.ExecuteReader();
                while (myreader1.Read())
                {
                    sNameFromtbl = myreader1.GetString(0);
                    sNameCombo.Items.Add(sNameFromtbl);
                }
                myreader1.Close();
               
            }

            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
        public void Fill_sAccessCombo()
        {
            sAccessCombo.Items.Add("Standard");
            sAccessCombo.Items.Add("Administrator");
        }
        public void Fill_sStatusCombo()
        {
            sStatusCombo.Items.Add("Enabled");
            sStatusCombo.Items.Add("Disabled");
        }

        private void sNameCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sNameCombo.Items.Clear();
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            dBConn.Open();
            SqlCommand LoadStaff = new SqlCommand("SELECT * FROM STAFF WHERE STAFF_NAME='" + sNameCombo.Text + "'", dBConn);
            SqlDataReader LoadStaff_DataReader = LoadStaff.ExecuteReader();
            try
            {
                while (LoadStaff_DataReader.Read())
                {
                    string sID = LoadStaff_DataReader["STAFF_ID"].ToString();
                    string sName = LoadStaff_DataReader["STAFF_NAME"].ToString();
                    string sUName = LoadStaff_DataReader["USERNAME"].ToString();
                    string sEmail = LoadStaff_DataReader["EMAIL"].ToString();
                    string sPass = LoadStaff_DataReader["PASSWORD"].ToString();
                    string sDesignation = LoadStaff_DataReader["DESIGNATION"].ToString();
                    string sAccess = LoadStaff_DataReader["ACCESS_LEVEL"].ToString();
                    string sStatus = LoadStaff_DataReader["STATUS"].ToString();
                    string sDepart = LoadStaff_DataReader["DEPART_ALIAS"].ToString();

                    sIDBox.Text = "00" + sID;
                    sNameBox.Text = sName;
                    sUsernameBox.Text = sUName;
                    sEmailBox.Text = sEmail;
                    sPasswordBox.Password = sPass;
                    sDesignationBox.Text = sDesignation;
                    sAccessCombo.Text = sAccess;
                    sStatusCombo.Text = sStatus;
                    sDeparmentBox.Text = sDepart;
                }
                LoadStaff_DataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
            Fill_sNameCombo();
        }

        private void sSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            SqlCommand SaveStaffCmd = new SqlCommand();
            dBConn.Open();

            string sName = sNameBox.Text;
            string sUName = sUsernameBox.Text;
            string sEmail = sEmailBox.Text;
            string sPass = sPasswordBox.Password;
            string sDesignation = sDesignationBox.Text;
            string sAccess = sAccessCombo.Text;
            string sStatus = sStatusCombo.Text;
            string sDepart = sDeparmentBox.Text;

            SaveStaffCmd.CommandText = "INSERT INTO STAFF (STAFF_NAME,USERNAME,EMAIL,PASSWORD,DESIGNATION,ACCESS_LEVEL," +
                "STATUS,DEPART_ALIAS) VALUES(@STAFF_NAME,@USERNAME,@EMAIL,@PASSWORD,@DESIGNATION,@ACCESS_LEVEL,@STATUS,@DEPART_ALIAS)";
            SaveStaffCmd.Parameters.AddWithValue("@STAFF_NAME", sName);
            SaveStaffCmd.Parameters.AddWithValue("@USERNAME", sUName);
            SaveStaffCmd.Parameters.AddWithValue("@EMAIL", sEmail);
            SaveStaffCmd.Parameters.AddWithValue("@PASSWORD", sPass);
            SaveStaffCmd.Parameters.AddWithValue("@DESIGNATION", sDesignation);
            SaveStaffCmd.Parameters.AddWithValue("@ACCESS_LEVEL", sAccess);
            SaveStaffCmd.Parameters.AddWithValue("@STATUS", sStatus);
            SaveStaffCmd.Parameters.AddWithValue("@DEPART_ALIAS", sDepart);
            SaveStaffCmd.Connection = dBConn;
            SaveStaffCmd.ExecuteNonQuery();
            
            dBConn.Close();
            dBConn.Dispose();
            MessageBox.Show("Successfull");
            Reset_Form();
            Fill_sNameCombo();
        }

        private void sUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            dBConn.Open();
            SqlCommand UpdateStaff_Cmd = new SqlCommand();
            UpdateStaff_Cmd.CommandText = "UPDATE STAFF SET STAFF_NAME = '"+ sNameBox.Text +"'," +
                "USERNAME = '" + sUsernameBox.Text + "', EMAIL = '" + sEmailBox.Text + "', PASSWORD = '" + sPasswordBox.Password + "'," +
                "DESIGNATION = '" + sDesignationBox.Text + "',ACCESS_LEVEL='" + sAccessCombo.Text + "',DEPART_ALIAS='" + sDeparmentBox.Text + "'," +
                "STATUS='" + sStatusCombo.Text + "' WHERE STAFF_ID='" + sIDBox.Text + "'";
            UpdateStaff_Cmd.Connection = dBConn;
            UpdateStaff_Cmd.ExecuteNonQuery();

            Fill_sNameCombo();
            Reset_Form();
            MessageBox.Show("Updated");
        }

        private void TabItemAll_GotFocus(object sender, RoutedEventArgs e)
        {
            Fill_Data_Grid_Class();
        }
        private void SessionTab_GotFocus(object sender, RoutedEventArgs e)
        {
            Fill_Sessions();
        }


        private void sDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            dBConn.Open();
            SqlCommand DeleteStaff = new SqlCommand("DELETE FROM STAFF WHERE STAFF_ID='"+ sIDBox.Text +"'",dBConn);
            SqlDataReader DeleteStaff_Reader;
            DeleteStaff_Reader = DeleteStaff.ExecuteReader();
            dBConn.Close();
            dBConn.Dispose();

            Fill_sNameCombo();
            Reset_Form();
            MessageBox.Show("Deleted");

        }
        public void Reset_Form()
        {
            sIDBox.Text = "";
            sNameBox.Text = "";
            sUsernameBox.Text = "";
            sEmailBox.Text = "";
            sPasswordBox.Password = "";
            sDesignationBox.Text = "";
            sAccessCombo.Text = "";
            sStatusCombo.Text = "";
            sDeparmentBox.Text = "";
        }
    }
}
