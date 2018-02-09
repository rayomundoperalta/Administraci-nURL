using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using Utility.ModifyRegistry;
using Globales;

/* Este Addin tiene por objetivo procesar un archivo xlsx de formato conocido, limpiarlo, i.e. quitar todos los carácteres
 * no imprimibles y subir la información del libro de excel a una base de datos
 * Excel va a ser llamado desde una aplicación ed consola o desde el administrador de tareas, es importante que no use
 * ventanas porque en producción no las va a tener y debe trabajar 100% desatendido
 * */
 
namespace ProcesoInformaciónAPF
{
    public partial class ThisAddIn
    {

        Cadenas g = new Cadenas();
        private ModifyRegistry myModifyRegistry;
        public int año;

        private void ProcesaArchivosXLSX()
        {
            string actualFilename = Globals.ThisAddIn.Application.ActiveWorkbook.FullName.Substring(Globals.ThisAddIn.Application.ActiveWorkbook.FullName.LastIndexOf('\\') + 1);
            int numeroDeRegistrosEnExcel = 0;
            int numeroDeColumnasEnExcel = 0;
            //int numeroDeRegistrosEnSql = 0;

            MessageBox.Show("Estamos en ProcesarArchivosXLSX - " + actualFilename, "D e b u g");

            /* Se debe encontrar el rango donde está la información, recordar que la primera fila
             * contiene los nombres de las columnas y las siguientes contienen los datos
             * */
            this.Application.Range["A1"].CurrentRegion.Select();
            Excel.Range rangoDatos = this.Application.ActiveWindow.RangeSelection;
            numeroDeRegistrosEnExcel = rangoDatos.Rows.Count;
            numeroDeColumnasEnExcel = rangoDatos.Columns.Count;
            MessageBox.Show("ROWS ---->>> " + rangoDatos.Rows.Count.ToString() + " <-----------> " + rangoDatos.Columns.Count.ToString(), "D E B U G");
            SqlConnection conn = new SqlConnection(g.ConnectionString());
            conn.Open();
            string comando;
            /* Hay que obtener el año de alguna forma */
            for (int i = 2; i <= numeroDeRegistrosEnExcel; i++)
            {
                comando = "EXECUTE [dbo].[InsertaContrato] " + (i - 1).ToString() + ", ";
                comando += año.ToString() + ", ";
                /*
                for (int j = 1; j <= numeroDeColumnasEnExcel; j++)
                {
                    string test = this.Application.ActiveSheet.Cells[i, j].Value.ToString();
                    MessageBox.Show("[" + i.ToString() + "," + j.ToString() + "] : " + test, "D A T O S");
                }
                */
                MessageBox.Show(comando, "Inserts to SQL");
                //SqlCommand command = new SqlCommand(comando, conn);
                //command.ExecuteNonQuery();
            }
            conn.Close();

            //Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);

            //Excel.Range firstElement = activeWorksheet.get_Range("A1").CurrentRegion.Select();
            //firstElement = firstElement.CurrentRegion.Select().Rows();


            //Excel.Range r = worksheet.get_Range("*1", Missing.Value);
            /*
            for (int j = 0; j < firstElement.Rows.Count; j++)
            {
                Excel.Range currentCell = firstElement.Rows[j + 1];
                MessageBox.Show(currentCell.ToString());
            }
            */

            /* La primera linea debe contener los nombres de las columnas */
            /*
            Excel.ListRows renglones = firstElement.CurrentRegion.Select();
            foreach(Excel.ListRow renglon in renglones)
            {
                MessageBox.Show(renglon.ToString());
            }
            */
            /*
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
            //Excel.Range firstRow;
            //Excel.Range newFirstRow;

            SqlConnection conn = new SqlConnection(g.ConnectionString());
            conn.Open();
            SqlCommand command = new SqlCommand("select * from [InformacionAPF].[dbo].[URLToBeDownloaded]", conn);
            SqlDataReader reader = command.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                MessageBox.Show(reader[0] + " -- " + reader[1], "Info para verificar");
            }
            conn.Close();
            */
            /*
            DialogResult iExit;
            iExit = MessageBox.Show("Activamos hoja", "Procesador Información APF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (iExit == DialogResult.Yes)
            {
                Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
                Excel.Range firstRow = activeWorksheet.get_Range("A1");
                firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Excel.Range newFirstRow = activeWorksheet.get_Range("A1");
                newFirstRow.Value2 = "This text was added by using code";
            }
            */
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //MessageBox.Show("InfoAPF Cargada", "Procesamiento InfoAPF");
            /* La llave del estado del add in se debe escribir solo si no existe en el registro */
            /* debe funcionar como un toggle */
            /* La primera vez el add in queda activado, después se acuerda del estado */
            MessageBox.Show("Estamos en StartUp", "D E B U G");
            myModifyRegistry = new ModifyRegistry(g.RegEditID());
            if (myModifyRegistry.Read(g.RegKeyEstado()) == null)
            {
                if (myModifyRegistry.Write(g.RegKeyEstado(), g.EstadoActivado()))
                {
                    
                    //MessageBox.Show("Info APF ACTIVADO", "Procesamiento InfoAPF");
                }
            }
            Globals.Ribbons.InfoAPF.checkBox1.Checked = myModifyRegistry.Read(g.RegKeyEstado()).Equals(g.EstadoActivado());

            if (myModifyRegistry.Read(g.RegKeyEstado()).Equals(g.EstadoActivado()))
            {
                this.Application.WorkbookBeforeSave += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeSaveEventHandler(Application_WorkbookBeforeSave);
                //MessageBox.Show("Bienvenido, Procesamiento APF", "Procesador Información APF");

                this.Application.WorkbookBeforeClose += Application_WorkbookBeforeClose1;

                this.Application.WorkbookActivate += Application_WorkbookActivate;
            }

            /*
            this.Application.Application.SheetActivate += new Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);
            */
            /*
            this.Application.WorkbookOpen += Application_WorkbookOpen;
            ((Excel.AppEvents_Event)Application).WorkbookOpen += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookOpenEventHandler(Application_WorkbookOpen);
            */
        }

        private void Application_WorkbookActivate(Excel.Workbook Wb)
        {
            MessageBox.Show("Estamos en Activate", "d e b u g");
            ProcesaArchivosXLSX();
        }

        private void Application_WorkbookBeforeClose1(Excel.Workbook Wb, ref bool Cancel)
        {
            //MessageBox.Show("Before Close", "D e b u g");
            

            /*
            DialogResult iExit;
            iExit = MessageBox.Show("Modificar archivo antes de salvar", "Procesador Información APF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (iExit == DialogResult.Yes)
            {
                Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
                Excel.Range firstRow = activeWorksheet.get_Range("A1");
                firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Excel.Range newFirstRow = activeWorksheet.get_Range("A1");
                newFirstRow.Value2 = "This text was added by using code";
            }

            
            connectionString = "Data Source=LAPTOP-BFFAQ78G;Initial Catalog=InformacionAPF;Persist Security Info=True;User ID=qc2;Password=1nt3rm3x.";
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
            Excel.Range firstRow;
            Excel.Range newFirstRow;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("select * from [InformacionAPF].[dbo].[URLToBeDownloaded] order by DownloadURL", conn);
            SqlDataReader reader = command.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                MessageBox.Show(reader[0].ToString(), "System Down", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            conn.Close();
            */
        }

        private void Application_WorkbookOpen(Excel.Workbook Wb)
        {
            //MessageBox.Show("WorkbookOpen", "Debug Info");
            
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_WorkbookAddinUninstall(object sender, System.EventArgs e)
        {
            //MessageBox.Show("U N I N S T A L L", "Addin");
        }

        void Application_SheetActivate(Object sh)
        {
            /*
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
            Excel.Range firstRow;
            Excel.Range newFirstRow;

            SqlConnection conn = new SqlConnection(g.ConnectionString());
            conn.Open();
            SqlCommand command = new SqlCommand("select * from [InformacionAPF].[dbo].[URLToBeDownloaded]", conn);
            SqlDataReader reader = command.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                firstRow = activeWorksheet.get_Range("A" + i.ToString());
                firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                newFirstRow = activeWorksheet.get_Range("A" + i.ToString());
                newFirstRow.Value2 = reader[0].ToString();
            }
            conn.Close();
            */
            /*
            DialogResult iExit;
            iExit = MessageBox.Show("Activamos hoja", "Procesador Información APF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (iExit == DialogResult.Yes)
            {
                Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
                Excel.Range firstRow = activeWorksheet.get_Range("A1");
                firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Excel.Range newFirstRow = activeWorksheet.get_Range("A1");
                newFirstRow.Value2 = "This text was added by using code";
            }
            */
        }

        void Application_WorkbookBeforeSave(Microsoft.Office.Interop.Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
        {
            //ProcesaArchivosXLSX();

            /*
            DialogResult iExit;
            iExit = MessageBox.Show("Modificar archivo antes de salvar", "Procesador Información APF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (iExit == DialogResult.Yes)
            {
                Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
                Excel.Range firstRow = activeWorksheet.get_Range("A1");
                firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                Excel.Range newFirstRow = activeWorksheet.get_Range("A1");
                newFirstRow.Value2 = "This text was added by using code";
            }

            
            connectionString = "Data Source=LAPTOP-BFFAQ78G;Initial Catalog=InformacionAPF;Persist Security Info=True;User ID=qc2;Password=1nt3rm3x.";
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
            Excel.Range firstRow;
            Excel.Range newFirstRow;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("select * from [InformacionAPF].[dbo].[URLToBeDownloaded] order by DownloadURL", conn);
            SqlDataReader reader = command.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                MessageBox.Show(reader[0].ToString(), "System Down", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            conn.Close();
            */
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}


