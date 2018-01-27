using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using AsYetUnnamed;

namespace AdministraciónURL
{
    public class Form1 : System.Windows.Forms.Form
    {
        private DataSet ds;
        private MultiColumnListBox listBox1;
        public Form1()
        {
            ds = DataArray.ToDataSet(new object[,]{
                    {"Row0, col0",  "Row0, col1" ,1},
                    {"Row00, col0", "Row1, col1" ,new object()},
                    {"Row1, col0",  "Row2, col1" ,"Some String"},
                    {"Row1a, col0", "Row3, col1" ,Rectangle.Empty},
                    {"row1aa,col0", "Row4, col1" ,1},
                    {"row0, col0",  "Row5, col1" ,1},
                    {"pow0, col0",  "Row6, col1" ,1},
                    {"Row7, col0",  "Row7, col1" ,"Hello from ExampleClass!!"},
                    {"Row8, col0",  "Row8, col1" ,"hola"}
                    });

            listBox1 = new MultiColumnListBox();
            listBox1.Parent = this;

            listBox1.DataSource = ds;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }
    }

    public partial class AdministraciónURLs : Form
    {
        SqlConnection connection;
        string connectionString;

        public AdministraciónURLs()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["AdministraciónURL.Properties.Settings.InformacionAPFConnectionString"].ConnectionString;
        }

        private void AdministraciónURLs_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'informacionAPFDataSet.URLToBeDownloaded' table. You can move, or remove it, as needed.
            this.uRLToBeDownloadedTableAdapter.Fill(this.informacionAPFDataSet.URLToBeDownloaded);
            PopulateURLs();
        }

        private void PopulateURLs()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("select * from [dbo].[URLToBeDownloaded] order by DownloadURL", connection))
            {
                DataTable URLdataTable = new DataTable();
                
                adapter.Fill(URLdataTable);

                //URLs.Items.AddRange(new object[] {
                //    "Item 1, DownloadURL",
                //    "Item 2, URLFiletype"});

                URLs.DisplayMember = "DownloadURL";
                
                URLs.DataSource = URLdataTable;
            }
        }

        private void URLs_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(URLs.SelectedValue.ToString());
        }
    }
}
