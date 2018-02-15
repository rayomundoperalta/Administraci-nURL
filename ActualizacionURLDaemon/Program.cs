using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Utility.Ecape;


namespace ActualizacionURLDaemon
{
    class Program
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["ActualizacionURLDaemon.Properties.Settings.InformacionAPFConnectionString"].ConnectionString;
        static string APFDataFiles = @"D:\CompraNetTemporaryDataFiles\";
        static System.IO.StreamWriter archivoPlano;

        private static void KillExcel()
        {
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
        }

        /* Cómo escribir a un archivo
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }

            Supongo que es necesario cerrar el archivo
        */

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                    Console.WriteLine("CTRL+C received!");
                    Console.WriteLine("Podemos hacer algo aqui");
                    break;
                case CtrlTypes.CTRL_BREAK_EVENT:
                    Console.WriteLine("CTRL+BREAK received!");
                    break;
                case CtrlTypes.CTRL_CLOSE_EVENT:
                    Console.WriteLine("Program being closed! ------------------------------>");
                    break;
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    Console.WriteLine("User is logging off!");
                    break;
            }
            return true;
        }

        static void LimpiaDirectorioTemporal()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("exec [dbo].[InicializaProcesaArchivo]", conn);
            command.ExecuteNonQuery();
            conn.Close();
            try
            {
                string[] fileList = Directory.GetFiles(APFDataFiles, "*.*");

                // List files.
                foreach (string f in fileList)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(APFDataFiles.Length);
                    Console.WriteLine("Cleanning - " + fName);
                    System.IO.File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
            catch (FileNotFoundException fileNotFound)
            {
                Console.WriteLine(fileNotFound.Message);
            }
        }

        private void App_WorkbookActivate(Excel.Workbook Wb)
        {
            Console.WriteLine("d e b u g: Estamos en Activate");
        }

        static void ExtraeRegistraXLSX(string fileName, int año)
        {
            /*
             * Esta rutina se va a encargar de procesar los archivos XLSX, es decir, limpiarlos y cargarlos a la BD
             * Suponemos que va a ser posible subir a la BD el contenido del xlsx sin mayor problema
             */
            
            /* Esta es la expresion regular que se necesita para escapear las comillas y las comas */
            /* Se inicializa la expresión regular para solo hacerlo una vez */
            string RegExp = @"(['])";
            Escape e = new Escape(RegExp);

            using (FileStream zipToOpen = new FileStream(fileName, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    Console.WriteLine("Numero de archivos en el zipfile: " + archive.Entries.Count.ToString());
                    foreach( ZipArchiveEntry entry in  archive.Entries)
                    {
                        Console.WriteLine("archivo: " + entry.FullName);
                        if (entry.FullName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Descomprimiendo " + entry.FullName);
                            entry.ExtractToFile(Path.Combine(APFDataFiles, entry.FullName));
                            /*
                            SqlConnection conn = new SqlConnection(connectionString);
                            conn.Open();
                            SqlCommand command = new SqlCommand("exec [dbo].[WriteProcesarArchivo] '" + entry.FullName + "'", conn);
                            command.ExecuteNonQuery();
                            conn.Close();
                            */

                            Excel.Workbook wb;
                            object oMissing1 = Type.Missing;
                            var app = new Microsoft.Office.Interop.Excel.Application();
                            // app.WorkbookActivate += Applic_WorkbookActivate;
                            wb = app.Workbooks.Open(Path.Combine(APFDataFiles, entry.FullName),
                                                    oMissing1, oMissing1, oMissing1, oMissing1,
                                                    oMissing1, oMissing1, oMissing1, oMissing1,
                                                    oMissing1, oMissing1, oMissing1, oMissing1,
                                                    oMissing1, oMissing1);
                            
                            //wb.ActiveSheet.Cells[1, 1] = 54;
                            Excel.Range rango = wb.ActiveSheet.Range("A1").CurrentRegion;
                            
                            Console.WriteLine("Valor " + wb.ActiveSheet.Cells[1, 1].Value.ToString() + " - renglones + 1 = " + rango.Rows.Count.ToString() + " - columnas =" + rango.Columns.Count.ToString());

                            int renglones = rango.Rows.Count;
                            int columnas = rango.Columns.Count;
                            SqlConnection conn = new SqlConnection(connectionString);
                            conn.Open();
                            string comando;
                            /* Hay que obtener el año de alguna forma */
                            for (int i = 2; i <= renglones; i++)
                            {
                                comando = "EXECUTE [dbo].[InsertaContrato] " + (i - 1).ToString() + ", ";
                                comando += año.ToString() + ", '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 1].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 2].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 3].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 4].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 5].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 6].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 7].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 8].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 9].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 10].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 11].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 12].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 13].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 14].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 15].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 16].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 17].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 18].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 19].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 20].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 21].Value.ToString()) + "', ";
                                string importeContrato = wb.ActiveSheet.Cells[i, 22].Value.ToString();
                                if (importeContrato.Length > 0)
                                {
                                    comando += importeContrato.ToString() + ", '";
                                }
                                else
                                {
                                    comando += 0 + ", '";
                                }
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 23].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 24].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 25].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 26].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 27].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 28].Value.ToString()) + "', ";
                                string aportaciónFederal = wb.ActiveSheet.Cells[i, 29].Value.ToString(); 
                                if (aportaciónFederal.Length > 0)
                                {
                                    comando += aportaciónFederal.ToString() + ", '";
                                } else
                                {
                                    comando += 0 +", '";
                                }
                                
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 30].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 31].Value.ToString()) + "', '";
                                comando += "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 32].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 33].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 34].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 35].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 36].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 37].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 38].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 39].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 40].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 41].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 42].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 43].Value.ToString()) + "', '";
                                comando += e.Reemplaza(wb.ActiveSheet.Cells[i, 44].Value.ToString()) + "'";
                                SqlCommand sqlCommand = new SqlCommand(comando, conn);
                                try
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    Console.WriteLine(comando);
                                    Console.WriteLine(ex.ToString());
                                    app.Quit();
                                }
                            }
                            wb.Close();
                            app.Quit();
                            Marshal.ReleaseComObject(wb);
                            Marshal.ReleaseComObject(app);
                            

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
                                MessageBox.Show(reader[0] + " -- " + reader[1], "Info para verificar");
                            }
                            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
                            Excel.Range firstRow = activeWorksheet.get_Range("A1");
                            firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                            Excel.Range newFirstRow = activeWorksheet.get_Range("A1");
                            newFirstRow.Value2 = "This text was added by using code";
                            conn.Close();
                            */

                        }
                    }
                }
            }
            
            Console.WriteLine("Deleting - " + fileName);
            try
            {
                System.IO.File.Delete(fileName);
            }
            catch (FileNotFoundException fileNotFoud)
            {
                Console.WriteLine(fileNotFoud.Message);
            }
        }

        private static void Applic_WorkbookActivate(Excel.Workbook Wb)
        {
            throw new NotImplementedException();
        }

        private static void ProcesaXLSXs()
        {
            //Console.WriteLine("Openning in Excel: " + APFDataFiles + entry.FullName);
            //Process.Start(APFDataFiles + entry.FullName).WaitForExit();
        }

        static void Main2(string[] args)
        {
            object oMissing = System.Reflection.Missing.Value;
                Excel.Workbook wb;
                object oMissing1 = Type.Missing;
                var app = new Microsoft.Office.Interop.Excel.Application();
                wb = app.Workbooks.Open(@"D:\CompraNetTemporaryDataFiles\Contratos2010_2012_160930120647.xlsx",
                                        oMissing1, oMissing1, oMissing1, oMissing1,
                                        oMissing1, oMissing1, oMissing1, oMissing1,
                                        oMissing1, oMissing1, oMissing1, oMissing1,
                                        oMissing1, oMissing1);
                wb.SaveAs(@"D:\CompraNetTemporaryDataFiles\ContratosSalvados.xlsx",
                                        Type.Missing, Type.Missing,
                                        Type.Missing, Type.Missing, Type.Missing,
                                        Excel.XlSaveAsAccessMode.xlExclusive,
                                        Type.Missing, Type.Missing, Type.Missing,
                                        Type.Missing, Type.Missing);
                                        // Excel.XlFileFormat.xlExcel12
                                       
                app.Quit();
                app.Quit();
        }

        static void Main(string[] args)
        {
            /*
             * Los archivos de compranet se actualizan al menos una vez al día
             * puede ser que con mayor frecuencia
             */

            /*
             * Vamos a automatizar la actualización de los datos de compranet y los archivos de interés de la APF
             * de una forma muy simple, bajamos los archivos, descomprimimos si es el caso y según el tipo de
             * archivo lo subimos a SQL. Esto lo vamos a hacer con .NET y los objetos de Office y no va a tener inteligencia ni artificiañ
             */

            /* Interceptamos las interrupciones que matan el proceso */
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            Console.WriteLine("Corriendo la actualización de los archivos de la APF");
            //Console.WriteLine(connectionString);

            LimpiaDirectorioTemporal();

            WebClient webClient = new WebClient();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("select * from [InformacionAPF].[dbo].[URLToBeDownloaded] order by DownloadURL", conn);
            SqlDataReader reader = command.ExecuteReader();
            string fileTitle, fileType, newFileTitle;
            int año;
            while (reader.Read())
            {
                fileTitle = reader[0].ToString();
                fileType = reader[1].ToString();
                if (!int.TryParse(reader[2].ToString(), out año))
                {
                    año = 0;
                };
                newFileTitle = APFDataFiles + fileTitle.Substring(fileTitle.LastIndexOf('/') + 1);
                Console.WriteLine("Descargando: " + fileTitle + " - " + fileType);
                Console.WriteLine(newFileTitle);
                webClient.DownloadFile(fileTitle, newFileTitle);
                // Ejemplo de como se zipea un directorio 
                // System.IO.Compression.ZipFile.CreateFromDirectory(@"c:\example\start", @"D:\CompraNetTemporaryDataFiles\Contratos2018.zip");
                //System.IO.Compression.ZipFile.ExtractToDirectory(newFileTitle, APFDataFiles);
                if (fileType.Equals("zip-xlsx", StringComparison.Ordinal))
                {
                    ExtraeRegistraXLSX(newFileTitle, año);
                } else if (fileType.Equals("", StringComparison.Ordinal)) {
                    Console.WriteLine("No se hace nada para: " + fileTitle);
                }
            }
            conn.Close();

            Console.WriteLine("F I N");
            Console.ReadKey();
            KillExcel();
            
            /*
            try
            {
                string[] fileList = Directory.GetFiles(APFDataFiles, "*.xlsx");

                // List files.
                foreach (string f in fileList)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(APFDataFiles.Length);
                    Console.WriteLine("Deleting - " + fName);
                    System.IO.File.Delete(f);
                }

                fileList = Directory.GetFiles(APFDataFiles, "*.*");

                // List files.
                foreach (string f in fileList)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(APFDataFiles.Length);
                    Console.WriteLine("Deleting - " + fName);
                    System.IO.File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
            */
        }

        #region unmanaged
        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
        #endregion
    }
}
