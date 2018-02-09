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

namespace AdministraciónURL
{
    
    public partial class AdministraciónURLs : Form
    {
        SqlConnection connection;
        string connectionString;
        String stringGetAllData = "select * from [InformacionAPF].[dbo].[URLToBeDownloaded] order by DownloadURL";

        public AdministraciónURLs()
        {
            InitializeComponent();

            Console.WriteLine("Empezamos ...");
            connectionString = ConfigurationManager.ConnectionStrings["AdministraciónURL.Properties.Settings.InformacionAPFConnectionString"].ConnectionString;
            Console.WriteLine(connectionString);
            
        }

        private void AdministraciónURLs_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'informacionAPFDataSet.URLToBeDownloaded' table. You can move, or remove it, as needed.
            //this.URLToBeDownloadedTableAdapter.Fill(this.informacionAPFDataSet.URLToBeDownloaded);
            PopulateURLs();
        }

        private void PopulateURLs()
        {
            ListViewItem renglon;

            listView1.Items.Clear();
            connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(stringGetAllData, connection);
            using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
            {
                while(sqlReader.Read())
                {
                    renglon = listView1.Items.Add(sqlReader[0].ToString());
                    renglon.SubItems.Add(sqlReader[1].ToString());
                    renglon.SubItems.Add(sqlReader[2].ToString());
                }
                
            }
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListViewItem item1;
            if (listView1.SelectedItems.Count > 0)
            {
                item1 = listView1.SelectedItems[0];
                String execString = "EXEC [dbo].[DeleteURL] @DownloadURL = N'" + item1.Text + "'";
                connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(execString, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                item1.Remove();
                PopulateURLs();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                String execString = "EXEC [dbo].[WriteURLToBeDownLoaded] @DownloadURL = N'" + textBox1.Text + "', @URLFileType = N'" + textBox2.Text + "', @AñoInfo = " + textBox3.Text;
                connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(execString, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            } 
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            PopulateURLs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult iExit;
            iExit = MessageBox.Show("Confirmar Salida", "Administración URL", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (iExit == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
