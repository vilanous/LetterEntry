using System;
using System.Linq;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections;

namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Window
    {
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30";
        public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30");
        public UserManagement()
        {
            InitializeComponent();
            Fill_Data_Grid_Class();
            Fill_sNameCombo();
        }

        public void Fill_Data_Grid_Class()
        {
            string CmdString = string.Empty;
            SqlConnection dBConn = new SqlConnection(dBPath);
            CmdString = "SELECT id as 'ID', Doc_Type as 'Doc type', Doc_Number as 'Number', Sent_By as 'From' FROM tableEntry ORDER BY ID DESC";
            SqlCommand cmd = new SqlCommand(CmdString, dBConn);
            SqlDataAdapter ada = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("tableEntry");
            ada.Fill(dt);
            AllUsersGrid.ItemsSource = dt.DefaultView;

        }
        public void Fill_sNameCombo()
        {
            //sNameCombo.Items.Clear();
            string sNameFromtbl;
            string Query1 = "SELECT STAFF_NAME FROM STAFF";
            SqlConnection dBConn = new SqlConnection(dBPath);
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

        private void sNameCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // sNameCombo.Items.Clear();
            dBConn.Open();
            SqlCommand LoadStaff = new SqlCommand("SELECT * FROM STAFF WHERE STAFF_NAME='" + sNameCombo.Text + "'", dBConn);
            SqlDataReader LoadStaff_DataReader = LoadStaff.ExecuteReader();
            try
            {
                while (LoadStaff_DataReader.Read())
                {
                    string sName = LoadStaff_DataReader["STAFF_NAME"].ToString();
                    string sUName = LoadStaff_DataReader["USERNAME"].ToString();
                    string sEmail = LoadStaff_DataReader["EMAIL"].ToString();
                    //string sPass = LoadStaff_DataReader["PASSWORD"].ToString();
                    string sDesignation = LoadStaff_DataReader["DESIGNATION"].ToString();
                    string sAccess = LoadStaff_DataReader["ACCESS_LEVEL"].ToString();
                    string sStatus = LoadStaff_DataReader["STATUS"].ToString();
                    string sDepart = LoadStaff_DataReader["DEPART_ALIAS"].ToString();

                    sNameBox.Text = sName;
                    sUsernameBox.Text = sUName;
                    sEmailBox.Text = sEmail;
                    sDesignationBox.Text = sDesignation;
                    sAccessLevelBox.Text = sAccess;
                    sStatusBox.Text = sStatus;
                    sDeparmentBox.Text = sDepart;
                }
                LoadStaff_DataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }

        private void sSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand SaveStaffCmd = new SqlCommand();
            dBConn.Open();
            string sName = sNameBox.Text;
            string sUName = sUsernameBox.Text;
            string sEmail = sEmailBox.Text;
            string sPass = sPasswordBox.Password;
            string sDesignation = sDesignationBox.Text;
            string sAccess = sAccessLevelBox.Text;
            string sStatus = sStatusBox.Text;
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
            Fill_sNameCombo();
        }
    }
}
