using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using Utility.ModifyRegistry;
using Globales;

namespace ProcesoInformaciónAPF
{
    public partial class ThisAddIn
    {

        Cadenas g = new Cadenas();
        private ModifyRegistry myModifyRegistry;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            MessageBox.Show("InfoAPF Cargada", "Procesamiento InfoAPF");
            /* La llave del estado del add in se debe escribir solo si no existe en el registro */
            /* debe funcionar como un toggle */
            /* La primera vez el add in queda activado, después se acuerda del estado */
            myModifyRegistry = new ModifyRegistry(g.RegEditID());
            if (myModifyRegistry.Read(g.RegKeyEstado()) == null)
            {
                if (myModifyRegistry.Write(g.RegKeyEstado(), g.EstadoActivado()))
                {
                    
                    MessageBox.Show("Info APF ACTIVADO", "Procesamiento InfoAPF");
                }
            }
            Globals.Ribbons.InfoAPF.checkBox1.Checked = myModifyRegistry.Read(g.RegKeyEstado()).Equals(g.EstadoActivado());
            
            /*
            this.Application.WorkbookBeforeSave += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeSaveEventHandler(Application_WorkbookBeforeSave);
            MessageBox.Show("Bienvenido, Procesamiento APF", "Procesador Información APF");

            this.Application.Application.SheetActivate += new Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);

            this.Application.WorkbookOpen += Application_WorkbookOpen;
            ((Excel.AppEvents_Event)Application).NewWorkbook += new Microsoft.Office.Interop.Excel.AppEvents_NewWorkbookEventHandler(Application_NewWorkbook);
            */
        }

        private void Application_WorkbookOpen(Excel.Workbook Wb)
        {
            MessageBox.Show("libro abierto " + Wb.Name, "Procesador Información APF");
            //throw new NotImplementedException
        }

        private void Application_NewWorkbook(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            MessageBox.Show("libro nuevo " + Wb.Name, "Procesador Información APF");
            //throw new NotImplementedException();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            /* esta rutina es llamada al momento de cerrar Excel, no debe cambiar el estado */
            //myModifyRegistry.DeleteKey(g.RegKeyEstado());
            //myModifyRegistry.DeleteSubKeyTree();
            //MessageBox.Show("InfoAPF Descargado", "Procesamiento InfoAPF");
        }

        private void ThisAddIn_WorkbookAddinUninstall(object sender, System.EventArgs e)
        {
            MessageBox.Show("U N I N S T A L L", "Addin");
        }

        void Application_SheetActivate(Object sh)
        {
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

            /*
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


