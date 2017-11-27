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
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        public Reports()
        {
            InitializeComponent();
            Fill_Reports();
            Load_ID();
            Load_Category();
            Load_Reference();
            Load_Contacts();
        }
        public void Fill_Reports()
        {
            try
            {
                SqlConnection dBConn = new SqlConnection();
                dBConn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();
                SqlCommand FillReports_Cmd = new SqlCommand("SELECT DOCUMENT_TYPE,DOC_REF,CONTACT_NAME,DETAILS,DEPART_NAME,DATE,STAFF_NAME FROM ENTRY", dBConn);
                SqlDataAdapter ReportssAdapter = new SqlDataAdapter(FillReports_Cmd);
                DataTable ReporsTable = new DataTable("ENTRY");
                ReportssAdapter.Fill(ReporsTable);
                ReportGrid.ItemsSource = ReporsTable.DefaultView;
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        public void Load_ID()
        {
            rIDCombo.Items.Clear();
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            SqlCommand LoadID_Cmd = new SqlCommand("SELECT ENTRY_ID FROM ENTRY", dBConn);

            SqlDataReader LoadIdReader;
            try
            {
                dBConn.Open();
                LoadIdReader = LoadID_Cmd.ExecuteReader();
                while (LoadIdReader.Read())
                {
                    string rIDFromtbl = LoadIdReader["ENTRY_ID"].ToString();
                    rIDCombo.Items.Add(rIDFromtbl);
                }
                LoadIdReader.Close();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
        public void Load_Category()
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            rCategoryCombo.Items.Clear();

            SqlCommand LoadCategory_Cmd = new SqlCommand("SELECT DOCUMENT_TYPE FROM DOCUMENT_TYPE", dBConn);

            SqlDataReader LoadCategoryReader;
            try
            {
                dBConn.Open();
                LoadCategoryReader = LoadCategory_Cmd.ExecuteReader();
                while (LoadCategoryReader.Read())
                {
                    string rCategoryFromtbl = LoadCategoryReader["DOCUMENT_TYPE"].ToString();
                    rCategoryCombo.Items.Add(rCategoryFromtbl);
                }
                LoadCategoryReader.Close();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
        public void Load_Reference()
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            rReferenceCombo.Items.Clear();

            SqlCommand LoadReference_Cmd = new SqlCommand("SELECT DOC_REF FROM ENTRY", dBConn);

            SqlDataReader LoadReferenceReader;
            try
            {
                dBConn.Open();
                LoadReferenceReader = LoadReference_Cmd.ExecuteReader();
                while (LoadReferenceReader.Read())
                {
                    string rReferenceFromtbl = LoadReferenceReader["DOC_REF"].ToString();
                    rReferenceCombo.Items.Add(rReferenceFromtbl);
                }
                LoadReferenceReader.Close();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
        public void Load_Contacts()
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            rContactCombo.Items.Clear();

            dBConn.Open();
            SqlCommand LoadContact_Cmd = new SqlCommand("SELECT Contact_Name FROM CONTACTS", dBConn);
            SqlDataReader LoadContactReader = LoadContact_Cmd.ExecuteReader();
            try
            {
                while (LoadContactReader.Read())
                {
                    rContactCombo.Items.Add(LoadContactReader["Contact_Name"]);
                }
                LoadContactReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }

        private void rIDCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //rIDCombo.Items.Clear();
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            dBConn.Open();
            
            try
            {
                SqlDataAdapter ReportssAdapter = new SqlDataAdapter
                    ("SELECT DOCUMENT_TYPE,DOC_REF,CONTACT_NAME,DETAILS,DEPART_NAME,DATE,STAFF_NAME FROM ENTRY WHERE ENTRY_ID='" + rIDCombo.Text + "'", dBConn);
                DataTable ReportsTable = new DataTable();
                ReportssAdapter.Fill(ReportsTable);
                ReportGrid.ItemsSource = ReportsTable.DefaultView;
                ReportssAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
            //Load_ID();
        }

        private void rCategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            dBConn.Open();

            try
            {
                SqlDataAdapter ReportssAdapter = new SqlDataAdapter
                    ("SELECT DOCUMENT_TYPE,DOC_REF,CONTACT_NAME,DETAILS,DEPART_NAME,DATE,STAFF_NAME FROM ENTRY WHERE DOCUMENT_TYPE='" + rCategoryCombo.Text + "'", dBConn);
                DataTable ReportsTable = new DataTable();
                ReportssAdapter.Fill(ReportsTable);
                ReportGrid.ItemsSource = ReportsTable.DefaultView;
                ReportssAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }

        private void rReferenceCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            dBConn.Open();

            try
            {
                SqlDataAdapter ReportssAdapter = new SqlDataAdapter
                    ("SELECT DOCUMENT_TYPE,DOC_REF,CONTACT_NAME,DETAILS,DEPART_NAME,DATE,STAFF_NAME FROM ENTRY WHERE DOC_REF='" + rReferenceCombo.Text + "'", dBConn);
                DataTable ReportsTable = new DataTable();
                ReportssAdapter.Fill(ReportsTable);
                ReportGrid.ItemsSource = ReportsTable.DefaultView;
                ReportssAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }

        private void rContactCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            dBConn.Open();

            try
            {
                SqlDataAdapter ReportssAdapter = new SqlDataAdapter
                    ("SELECT DOCUMENT_TYPE,DOC_REF,CONTACT_NAME,DETAILS,DEPART_NAME,DATE,STAFF_NAME FROM ENTRY WHERE CONTACT_NAME='" + rContactCombo.Text + "'", dBConn);
                DataTable ReportsTable = new DataTable();
                ReportssAdapter.Fill(ReportsTable);
                ReportGrid.ItemsSource = ReportsTable.DefaultView;
                ReportssAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }

        private void rDepartCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            dBConn.Open();

            try
            {
                SqlDataAdapter ReportssAdapter = new SqlDataAdapter
                    ("SELECT DOCUMENT_TYPE,DOC_REF,CONTACT_NAME,DETAILS,DEPART_NAME,DATE,STAFF_NAME FROM ENTRY WHERE DEPART_NAME='" + rDepartCombo.Text + "'", dBConn);
                DataTable ReportsTable = new DataTable();
                ReportssAdapter.Fill(ReportsTable);
                ReportGrid.ItemsSource = ReportsTable.DefaultView;
                ReportssAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }

        private void rDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection dBConn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ToString()
            };
            dBConn.Open();

            try
            {
                SqlDataAdapter ReportssAdapter = new SqlDataAdapter
                    ("SELECT DOCUMENT_TYPE,DOC_REF,CONTACT_NAME,DETAILS,DEPART_NAME,DATE,STAFF_NAME FROM ENTRY WHERE DATE='" + rDate.Text + "'", dBConn);
                DataTable ReportsTable = new DataTable();
                ReportssAdapter.Fill(ReportsTable);
                ReportGrid.ItemsSource = ReportsTable.DefaultView;
                ReportssAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dBConn.Close();
        }
    }
}
