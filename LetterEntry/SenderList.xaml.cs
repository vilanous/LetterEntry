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
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for SenderList.xaml
    /// </summary>
    public partial class SenderList : Window
    {
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30";
        //public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30");
        public SenderList()
        {
            InitializeComponent();
            Fill_Combo_Box();
        }
        public void Fill_Combo_Box()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            Sender_Name_Combo.Items.Clear();
            dBConn.Open();
            SqlCommand Load_Sender_Name_Cmd = new SqlCommand("SELECT Contact_Name FROM CONTACTS", dBConn);
            SqlDataReader Sender_Name_DataReader = Load_Sender_Name_Cmd.ExecuteReader();
            try
            {
                while (Sender_Name_DataReader.Read())
                {
                    Sender_Name_Combo.Items.Add(Sender_Name_DataReader["Contact_Name"]);
                }
                Sender_Name_DataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
            
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            if (Sender_Name_Combo.Text.Length > 0)
            {
                string save_Email = EmailBox.Text;
                string save_Contact = Convert.ToString(ContactBox.Text);
                string save_Sender = Sender_Name_Combo.Text;
                string save_Address = cAddressBox.Text;
                string Record_Load_String = ("SELECT * FROM CONTACTS WHERE CONVERT (VARCHAR, Contact_Name) = @save_Sender");
                SqlCommand Load_Record_Cmd = new SqlCommand(Record_Load_String, dBConn);

                Load_Record_Cmd.Parameters.Add("save_Sender", SqlDbType.VarChar).Value = save_Sender;

                DataSet Senders = new DataSet();
                SqlDataAdapter dataAdapter;
                try
                {
                    dBConn.Open();
                    dataAdapter = new SqlDataAdapter(Load_Record_Cmd);
                    dataAdapter.Fill(Senders);

                    if (Senders.Tables[0].Rows.Count > 0)
                    {
                        Notification_Box.Text = "Record Already Exists";
                    }
                    else
                    {
                        SqlCommand save = new SqlCommand();

                        save.CommandText = "INSERT INTO CONTACTS (Contact_Name,Email_Address,Contact_Number,ADDRESS) VALUES(@Contact_Name,@Email_Address,@Contact_Number,@ADDRESS)";
                        save.Parameters.AddWithValue("@Contact_Name", save_Sender);
                        save.Parameters.AddWithValue("@Email_Address", save_Email);
                        save.Parameters.AddWithValue("@Contact_Number", save_Contact);
                        save.Parameters.AddWithValue("@ADDRESS", save_Address);
                        save.Connection = dBConn;
                        save.ExecuteNonQuery();

                        Notification_Box.Text = "Record Saved";
                        Sender_Name_Combo.Text = "";
                        EmailBox.Text = "";
                        ContactBox.Text = "";
                        cAddressBox.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dBConn.Close();
                Fill_Combo_Box();
                Update_MainCombo();
            }
            else
            {
                Notification_Box.Text = "Insert a Name";
            }
           
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            if (cContactNameBox.Text.Length > 0)
            {
                string Del_Name = cContactNameBox.Text;
                string Del_Email = EmailBox.Text;
                string Del_Contact = ContactBox.Text;
                string Del_Address = cAddressBox.Text;
                SqlConnection dBconn = new SqlConnection(dBPath);
                //SqlCommand delete_Sender = new SqlCommand();

                dBconn.Open();

                string Query = "DELETE FROM CONTACTS WHERE Contact_Name='" + cContactNameBox.Text + "'";
                SqlCommand DeleteCmd = new SqlCommand(Query, dBconn);
                DeleteCmd.ExecuteNonQuery();
                dBconn.Close();
                Notification_Box.Text = "Record Deleted";
            }
            else
            {
                Notification_Box.Text = "Nothing Selected";
            }
            Fill_Combo_Box();
            Update_MainCombo();
        }
        public void Update_MainCombo()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            MainWindow.AppWindow.sentByBox.Items.Clear();

            string Sender_Name_From_Tbl;
            SqlCommand Load_Sender_Name_Cmd = new SqlCommand("SELECT Contact_Name FROM CONTACTS", dBConn);
            SqlDataReader Sender_Name_DataReader;
            try
            {
                dBConn.Open();
                Sender_Name_DataReader = Load_Sender_Name_Cmd.ExecuteReader();
                while (Sender_Name_DataReader.Read())
                {
                    Sender_Name_From_Tbl = Sender_Name_DataReader.GetString(0);
                    MainWindow.AppWindow.sentByBox.Items.Add(Sender_Name_From_Tbl);
                }
                Sender_Name_DataReader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }

        private void Sender_Name_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            Sender_Name_Combo.Items.Clear();
            dBConn.Open();
            SqlCommand Load_Sender_Name_Cmd = new SqlCommand("SELECT * FROM CONTACTS WHERE Contact_Name='"+ Sender_Name_Combo.Text +"'", dBConn);
            SqlDataReader Sender_Name_DataReader = Load_Sender_Name_Cmd.ExecuteReader();
            try
            {
                while (Sender_Name_DataReader.Read())
                {
                    cContactNameBox.Text = Sender_Name_DataReader["Contact_name"].ToString();
                    EmailBox.Text = Sender_Name_DataReader["EMAIL_ADDRESS"].ToString();
                    ContactBox.Text = Sender_Name_DataReader["Contact_Number"].ToString();
                    cAddressBox.Text = Sender_Name_DataReader["Address"].ToString();
                }
                Sender_Name_DataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
            Fill_Combo_Box();
        }
    }
}
