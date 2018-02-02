using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Data.SqlClient;

namespace ProcesoInformaciónAPF
{
    public partial class ThisAddIn
    {
        string connectionString;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            
            this.Application.WorkbookBeforeSave += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeSaveEventHandler(Application_WorkbookBeforeSave);
            MessageBox.Show("Bienvenido, Procesamiento APF", "Procesador Información APF");

            this.Application.Application.SheetActivate += new Excel.AppEvents_SheetActivateEventHandler(Application_SheetActivate);

            this.Application.WorkbookOpen += Application_WorkbookOpen;
            ((Excel.AppEvents_Event)Application).NewWorkbook += new Microsoft.Office.Interop.Excel.AppEvents_NewWorkbookEventHandler(Application_NewWorkbook);
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
        }

        void Application_SheetActivate(Object sh)
        {
            connectionString = "Data Source=LAPTOP-BFFAQ78G;Initial Catalog=InformacionAPF;Persist Security Info=True;User ID=qc2;Password=1nt3rm3x.";
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
            Excel.Range firstRow;
            Excel.Range newFirstRow;

            SqlConnection conn = new SqlConnection(connectionString);
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


