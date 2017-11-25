using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using LetterEntry;
using System.Configuration;

namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for DocumentType.xaml
    /// </summary>
    public partial class DocumentType : Window
    {
        public String getdoctype;
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30";
        //public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30");
        public DocumentType()
        {
            InitializeComponent();
            FillComboClass();
        }
        public void FillComboClass()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            Document_Type_Combo.Items.Clear();
            dBConn.Open();
            SqlCommand Load_Document_Type_Cmd = new SqlCommand("SELECT DOCUMENT_TYPE FROM DOCUMENT_TYPE", dBConn);
            SqlDataReader Document_Type_DataReader = Load_Document_Type_Cmd.ExecuteReader();
            try
            {
                while (Document_Type_DataReader.Read())
                {
                    Document_Type_Combo.Items.Add(Document_Type_DataReader["DOCUMENT_TYPE"]);
                }
                Document_Type_DataReader.Close();
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
            string savedoctype = Document_Type_Combo.Text;
            string Record_Load_String = ("SELECT * FROM DOCUMENT_TYPE WHERE CONVERT (VARCHAR, DOCUMENT_TYPE) = @savedoctype");
            SqlCommand Load_Record_Cmd = new SqlCommand(Record_Load_String, dBConn);

            Load_Record_Cmd.Parameters.Add("savedoctype", SqlDbType.VarChar).Value = savedoctype;

            DataSet Documents = new DataSet();
            SqlDataAdapter dataAdapter;
            try
            {
                dBConn.Open();
                dataAdapter = new SqlDataAdapter(Load_Record_Cmd);
                dataAdapter.Fill(Documents);

                if (Documents.Tables[0].Rows.Count > 0)
                {
                    Notification_Box.Text = "Record Already Exists";
                }
                else
                {
                    SqlCommand save = new SqlCommand();

                    save.CommandText = "INSERT INTO DOCUMENT_TYPE (DOCUMENT_TYPE) VALUES(@DOCUMENT_TYPE)";
                    save.Parameters.AddWithValue("@DOCUMENT_TYPE", savedoctype);
                    save.Connection = dBConn;
                    save.ExecuteNonQuery();

                    Notification_Box.Text = "Record Saved";
                    Document_Type_Combo.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
            try
            {
                FillComboClass();
                Update_MainCombo();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            string DelTemp = Document_Type_Combo.Text;
            SqlConnection dBconn = new SqlConnection(dBPath);
            SqlCommand deletedoctype = new SqlCommand();

            dBconn.Open();

            deletedoctype.CommandText = "DELETE FROM DOCUMENT_TYPE WHERE DOCUMENT_TYPE = @DOCUMENT_TYPE";
            deletedoctype.Parameters.AddWithValue("@DOCUMENT_TYPE", DelTemp);
            deletedoctype.Connection = dBconn;
            deletedoctype.ExecuteNonQuery();

            dBconn.Close();
            Notification_Box.Text = "Record Deleted";
            FillComboClass();
            Update_MainCombo();
        }
        public void Update_MainCombo()
        {
            SqlConnection dBConn = new SqlConnection();
            dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
            //((MainWindow)Application.Current.MainWindow).comboDocType.Items.Clear();
            MainWindow.AppWindow.comboDocType.Items.Clear();

            string getdoctype;
            SqlCommand Load_Doc_Type_Cmd = new SqlCommand("SELECT DOCUMENT_TYPE FROM DOCUMENT_TYPE", dBConn);
            SqlDataReader Document_Type_DataReader;
            try
            {
                dBConn.Open();
                Document_Type_DataReader = Load_Doc_Type_Cmd.ExecuteReader();
                while (Document_Type_DataReader.Read())
                {
                    getdoctype = Document_Type_DataReader.GetString(0);
                    //((MainWindow)Application.Current.MainWindow).comboDocType.Items.Add(getdoctype);
                    MainWindow.AppWindow.comboDocType.Items.Add(getdoctype);
                }
                Document_Type_DataReader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
        private void On_TextChanged(object sender, RoutedEventArgs e)
        {
            Notification_Box.Text = "";
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            //((MainWindow)Application.Current.MainWindow).comboDocType.Items.Add(getdoctype);
        }
    }
}
