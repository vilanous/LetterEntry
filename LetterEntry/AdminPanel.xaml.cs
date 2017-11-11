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
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;


namespace LetterEntry
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public static string Database_Path = File.ReadLines(@"C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
        public String dBPath = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30";
        public SqlConnection dBConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Database_Path + "; UID=sa; Password=welcome@123; Connect Timeout = 30");
        public AdminPanel()
        {
            InitializeComponent();
            CheckFordatabase();
        }
        private void Add_Staff_Class()
        {
            string NewStaff = Add_Staff_Box.Text;
            dBConn.Open();
            SqlCommand Add_New_Staff_Cmd = new SqlCommand();
            Add_New_Staff_Cmd.CommandText = "INSERT INTO tableStaff (Staff_Name) VALUES(@Staff_Name)";
            Add_New_Staff_Cmd.Parameters.AddWithValue("@Staff_Name", NewStaff);
            Add_New_Staff_Cmd.Connection = dBConn;
            Add_New_Staff_Cmd.ExecuteNonQuery();

            ((MainWindow)Application.Current.MainWindow).entryBybox.Items.Clear();
            ((MainWindow)Application.Current.MainWindow).entryBybox.Items.Add(NewStaff);
            dBConn.Close();
        }
        public void CheckFordatabase()
        {
            if (File.Exists("C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf"))
            {
                string readText = File.ReadLines("C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf").First();
                Database_Filepath.Text = readText;
            }
            else
            {
                Database_Filepath.Text = "Database Not found,";
            }
            if (File.Exists("C:\\Program Files (x86)\\GIS-ENTRY\\GIS-ENTRY.xls"))
            {
                Export_Database_Box.Text = "C:\\Program Files (x86)\\GIS-ENTRY\\GIS-ENTRY.xls";
            }
            else
            {
                Export_Database_Box.Text = "Click 'Export'";
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog DbFinder = new Microsoft.Win32.OpenFileDialog();


            // Set filter for file extension and default file extension 
            DbFinder.DefaultExt = ".mdf";
            DbFinder.Filter = "SQL Database File (*.mdf)|*.mdf";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = DbFinder.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = DbFinder.FileName;
                Database_Filepath.Text = filename;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string pathtofile = Database_Filepath.Text;
            StreamWriter file = new StreamWriter("C:\\Program Files (x86)\\GIS-ENTRY\\Database_Path.inf");
            file.WriteLine(pathtofile);
            file.Close();
        }


        private void Button_Export_Database_Click(object sender, RoutedEventArgs e)
        {
            string data = null;
            int i = 0;
            int j = 0;
            dBConn.Open();

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            SqlDataAdapter DbExportAdapter = new SqlDataAdapter("SELECT * FROM tableEntry", dBConn);
            DataSet DbExport_DS = new DataSet();
            DbExportAdapter.Fill(DbExport_DS);

            for (i = 0; i <= DbExport_DS.Tables[0].Rows.Count - 1; i++)
            {
                for (j = 0; j <= DbExport_DS.Tables[0].Columns.Count - 1; j++)
                {
                    data = DbExport_DS.Tables[0].Rows[i].ItemArray[j].ToString();
                    xlWorkSheet.Cells[i + 1, j + 1] = data;
                }
            }

            xlWorkBook.SaveAs("C:\\Program Files (x86)\\GIS-ENTRY\\GIS-Entry.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Excel file created ,C:\\Program Files (x86)\\GIS-ENTRY\\GIS-Entry.xls");
            Export_Database_Box.Text = "C:\\Program Files (x86)\\GIS-ENTRY\\GIS-Entry.xls";
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void Button_Add_Staff_Click(object sender, RoutedEventArgs e)
        {
            Add_Staff_Class();
        }
    }
}
