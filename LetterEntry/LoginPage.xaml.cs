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
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;

namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; Integrated Security = True; Connect Timeout = 30";
        //public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; Integrated Security = True; Connect Timeout = 30");
        public LoginPage()
        {
            InitializeComponent();
            
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(UserNameBox.Text != "" & PassWordBox.Password != "")
            {
                string SessionTime = DateTime.Now.ToShortTimeString();
                SqlCommand CurrentUser_Cmd;
                SqlCommand Login_Cmd;
                SqlDataReader UserDataReader;

                SqlConnection dBConn = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
                };
                dBConn.Open();
                Login_Cmd = new SqlCommand("select USERNAME,PASSWORD from Staff where Username=@USERNAME and PASSWORD=@PASSWORD", dBConn);
                Login_Cmd.Parameters.AddWithValue("@USERNAME", UserNameBox.Text.ToString());
                Login_Cmd.Parameters.AddWithValue("@PASSWORD", PassWordBox.Password.ToString());
                UserDataReader = Login_Cmd.ExecuteReader();
                
                if (UserDataReader.HasRows)
                {
                    UserDataReader.Dispose();

                    SqlDataReader CUserDataReader;
                    CurrentUser_Cmd = new SqlCommand ("INSERT INTO SESSION (SESSION_TIME,USERNAME,ACCESS_LEVEL)" +
                        " VALUES('"+ SessionTime +"','"+ UserNameBox.Text.ToString() + 
                        "', (SELECT ACCESS_LEVEL FROM STAFF WHERE USERNAME = '"+ UserNameBox.Text.ToString() +"'))");
                    CurrentUser_Cmd.Connection = dBConn;
                    CUserDataReader = CurrentUser_Cmd.ExecuteReader();
                    MainWindow NewMainInstance = new MainWindow();
                    Close();
                    dBConn.Close();
                    NewMainInstance.ShowDialog();

                }
                else
                {
                    MessageBox.Show("no user available");
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
