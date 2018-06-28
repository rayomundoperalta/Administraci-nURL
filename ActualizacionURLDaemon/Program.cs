using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Utility.Ecape;
using Microsoft.SqlServer.Dts.Runtime;


namespace ActualizacionURLDaemon
{
    class Program
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["ActualizacionURLDaemon.Properties.Settings.InformacionAPFConnectionString"].ConnectionString;
        static string APFDataFiles = ConfigurationManager.ConnectionStrings["ActualizacionURLDaemon.Properties.Settings.APFDataFiles"].ConnectionString;
        static string Excel2TxtPython = ConfigurationManager.ConnectionStrings["ActualizacionURLDaemon.Properties.Settings.Excel2TxtPython"].ConnectionString;

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
            /*
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("exec [dbo].[InicializaProcesaArchivo]", conn);
            command.ExecuteNonQuery();
            conn.Close();
            */
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

        static void runPythonScript(string cmd, string args)
        {
            Process myProcess = new Process();
            string Arguments = cmd + " " + args;

            Console.WriteLine("Run Python Script");
            Console.WriteLine(Arguments);
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                // You can start any process, HelloWorld is a do-nothing example.
                myProcess.StartInfo.FileName = Excel2TxtPython;
                myProcess.StartInfo.Arguments = Arguments;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                // This code assumes the process you are starting will terminate itself. 
                // Given that is is started without a window so you cannot terminate it 
                // on the desktop, it must terminate itself or you can do it programmatically
                // from this application using the Kill method.
                myProcess.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Termino Python");
        }

        static void CleanningAPFExcelTxtFile(string InputFileName, string OutputFileName)
        {
            byte[] textArrayOfBytes = File.ReadAllBytes(InputFileName);
            long[] frecuencia = new long[256];
            byte[] convertidor = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                convertidor[i] = (byte)i;
            }
            for (int i = 127; i < 161; i++)
            {
                convertidor[i] = 32;
            }
            for (int i = 0; i < 9; i++)
            {
                convertidor[i] = 32;
            }
            convertidor[11] = 32;
            convertidor[12] = 32;
            convertidor[44] = 32;
            convertidor[9]   = 44;
            for (int i = 14; i < 32; i++)
            {
                convertidor[i] = 32;
            }
            for (int i = 0; i < 256; i++)
            {
                frecuencia[i] = 0;
            }
            Console.WriteLine("Tamaño del archivo  " + textArrayOfBytes.Length);
            Console.WriteLine("Antes de la limpieza");
            for (long i = 0; i < textArrayOfBytes.Length; i++)
            {
                frecuencia[textArrayOfBytes[i]]++;
            }
            for (int i = 0; i < 256; i += 4)
            {
                Console.WriteLine("{0,3} - {1,10}   {2,3} - {3,10}   {4,3} - {5,10}   {6,3} - {7,10}   ",
                    i, frecuencia[i].ToString(),
                    i + 1, frecuencia[i + 1].ToString(),
                    i + 2, frecuencia[i + 2].ToString(),
                    i + 3, frecuencia[i + 3].ToString());
                frecuencia[i] = 0;
                frecuencia[i + 1] = 0;
                frecuencia[i + 2] = 0;
                frecuencia[i + 3] = 0;
            }
            Console.WriteLine("Después de la limpieza");
            for (long i = 0; i < textArrayOfBytes.Length; i++)
            {
                textArrayOfBytes[i] = convertidor[textArrayOfBytes[i]];
                frecuencia[textArrayOfBytes[i]]++;
            }
            for (int i = 0; i < 256; i += 4)
            {
                Console.WriteLine("{0,3} - {1,10}   {2,3} - {3,10}   {4,3} - {5,10}   {6,3} - {7,10}   ",
                    i, frecuencia[i].ToString(),
                    i + 1, frecuencia[i + 1].ToString(),
                    i + 2, frecuencia[i + 2].ToString(),
                    i + 3, frecuencia[i + 3].ToString());
            }
            File.WriteAllBytes(OutputFileName, textArrayOfBytes);
            string pkgLocation;
            Package pkg;
            Application app;
            DTSExecResult pkgResults;

            pkgLocation = @"D:\VSProjects\AdministraciónURL\ISPAPF1\ISPAPF1\" +  Path.GetFileNameWithoutExtension(OutputFileName) + ".dtsx";
            app = new Application();
            pkg = app.LoadPackage(pkgLocation, null);
            pkgResults = pkg.Execute();
        }

        static void ExtraeRegistraXLSX(string fileName, int año)
        {
            /*
             * Esta rutina se va a encargar de procesar los archivos XLSX, es decir, limpiarlos y cargarlos a la BD
             * Suponemos que va a ser posible subir a la BD el contenido del xlsx sin mayor problema
             */
            
            /* Esta es la expresion regular que se necesita para escapear las comillas y las comas */
            /* Se inicializa la expresión regular para solo hacerlo una vez */
            //string RegExp = @"(['])";
            //Escape e = new Escape(RegExp);

            string fullName = null;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using (FileStream zipToOpen = new FileStream(fileName, FileMode.Open))
            {
                string ZipFileName = Path.GetFileNameWithoutExtension(fileName);
                Console.WriteLine("------------------> " + fileName + " !! " + ZipFileName);
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    Console.WriteLine("Numero de archivos en el zipfile: " + archive.Entries.Count.ToString());
                    if (archive.Entries.Count < 2)
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            string FileNameSinExt = Path.GetFileNameWithoutExtension(entry.FullName);
                            string FechaDeActualización = FileNameSinExt.Substring(FileNameSinExt.Length - 12);
                            Console.WriteLine("archivo: " + entry.FullName + " sin ext: " + FileNameSinExt + " fecha hora: " + FechaDeActualización);
                            
                            ////////////////
                            SqlCommand command = new SqlCommand("select SFPLastUpdate from [InformacionAPF].[dbo].[SFPFechaActualización] where FileName = '" + ZipFileName + "'", conn);
                            SqlDataReader reader = command.ExecuteReader();
                            string SFPLastUpdate;
                            bool ProcesarArchivo = false;
                            if (reader.HasRows)
                            {
                                reader.Read();
                                SFPLastUpdate = reader[0].ToString();
                                reader.Close();
                                if (Convert.ToInt64(FechaDeActualización) > Convert.ToInt64(SFPLastUpdate))
                                {
                                    ProcesarArchivo = true;
                                    command = new SqlCommand("UPDATE [InformacionAPF].[dbo].[SFPFechaActualización] SET SFPLastUpdate = " + FechaDeActualización + " where FileName = " + ZipFileName, conn);
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                reader.Close();
                                ProcesarArchivo = true;
                                command = new SqlCommand("INSERT INTO [InformacionAPF].[dbo].[SFPFechaActualización] (FileName, SFPLastUpdate) VALUES ('" + ZipFileName + "', '" + FechaDeActualización + "')", conn);
                                command.ExecuteNonQuery();
                            }
                            ////////////////
                            if (ProcesarArchivo)
                            {
                                if (entry.FullName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                                {
                                    //object oMissing = System.Reflection.Missing.Value;
                                    Console.WriteLine("Descomprimiendo " + entry.FullName);
                                    fullName = Path.Combine(APFDataFiles, entry.FullName);
                                    Console.WriteLine(fullName);
                                    entry.ExtractToFile(fullName);

                                    runPythonScript("D:\\VSProjects\\AdministraciónURL\\ActualizacionURLDaemon\\xlsx\\excel2txt.py", fullName + " " + APFDataFiles + "archivo.txt");
                                    CleanningAPFExcelTxtFile(APFDataFiles + "archivo.txt",
                                        APFDataFiles + Path.GetFileNameWithoutExtension(fileName) + ".txt");
                                    Console.WriteLine("--------------> " + APFDataFiles + " ++++ " + entry.FullName);
                                    Console.WriteLine("Deleting xlsx file - " + fullName);
                                    try
                                    {
                                        System.IO.File.Delete(fullName);
                                    }
                                    catch (FileNotFoundException fileNotFoud)
                                    {
                                        Console.WriteLine(fileNotFoud.Message);
                                    }
                                    
                                }
                            }
                        }
                    }
                    else
                        Console.WriteLine("E R R O R, hay mas de un archivo en el archivo zip");
                }
            }
            Console.WriteLine("Deleting zip file - " + fileName);
            try
            {
                System.IO.File.Delete(fileName);
            }
            catch (FileNotFoundException fileNotFoud)
            {
                Console.WriteLine(fileNotFoud.Message);
            }
            Console.WriteLine("Deleting temporary file - archivo.txt");
            try
            {
                System.IO.File.Delete(APFDataFiles + "archivo.txt");
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
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

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

            //FreeConsole(); // closes the console

            /* Interceptamos las interrupciones que matan el proceso */
            //SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

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
                Console.WriteLine("newFileTitle: " + newFileTitle);
                webClient.DownloadFile(fileTitle, newFileTitle);
                // Ejemplo de como se zipea un directorio 
                // System.IO.Compression.ZipFile.CreateFromDirectory(@"c:\example\start", @"D:\CompraNetTemporaryDataFiles\Contratos2018.zip");
                //System.IO.Compression.ZipFile.ExtractToDirectory(newFileTitle, APFDataFiles);
                if (fileType.Equals("zip-xlsx", StringComparison.Ordinal))
                {
                    ExtraeRegistraXLSX(newFileTitle, año);
                }
                else
                {
                    if (fileType.Equals("", StringComparison.Ordinal))
                    {
                        Console.WriteLine("No se hace nada para: " + fileTitle);
                    }
                }

            }
            conn.Close();

            Console.Write("Descargados los archivos de CompraNet\nEnter to finish: ");
            //Console.ReadKey();
            
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

                fileList = Directory.GetFiles(APFDataFiles, "*.zip");

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
