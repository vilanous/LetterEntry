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

namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; Integrated Security = True; Connect Timeout = 30";
        public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; Integrated Security = True; Connect Timeout = 30");
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //SqlCommand Login_Cmd;
            //SqlDataReader UserDataReader;
            //Login_Cmd = new SqlCommand("select Username,word from tableStaff where USername=@Username and word=@word", dBConn);
            //Login_Cmd.Parameters.AddWithValue("@Username", UserNameBox.Text.ToString());
            //Login_Cmd.Parameters.AddWithValue("@word", PassWordBox.Password.ToString());
            //UserDataReader = Login_Cmd.ExecuteReader();
            if (UserNameBox.Text == "admin" && PassWordBox.Password == "admin")
            {
                
                MainWindow NewMainInstance = new MainWindow();
                //((MainWindow)Application.Current.MainWindow).Admin_Button.IsEnabled.Equals(false);
                this.Close();
                
                NewMainInstance.ShowDialog();
                
            }
            else
            {
                
                //if (UserDataReader.HasRows)
                //{
                //    UserDataReader.Close();
                //    MessageBox.Show("Welcome- The Username and word is Correct", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                //}
                //else
                //{
                //    MessageBox.Show("Invalid Username or word", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
