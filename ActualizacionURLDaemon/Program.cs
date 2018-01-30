using System;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;


namespace ActualizacionURLDaemon
{
    class Program
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["ActualizacionURLDaemon.Properties.Settings.InformacionAPFConnectionString"].ConnectionString;
        static string APFDataFiles = @"D:\CompraNetTemporaryDataFiles";

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

            Console.WriteLine("Corriendo la actualización de los archivos de la APF");
            Console.WriteLine(connectionString);
            WebClient webClient = new WebClient();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand("select * from [InformacionAPF].[dbo].[URLToBeDownloaded] order by DownloadURL", conn);
            SqlDataReader reader = command.ExecuteReader();
            string fileTitle, fileType, newFileTitle;
            while (reader.Read())
            {
                fileTitle = reader[0].ToString();
                fileType = reader[1].ToString();
                newFileTitle = APFDataFiles + fileTitle.Substring(fileTitle.LastIndexOf('/') + 1);
                Console.WriteLine("Descargando: " + fileTitle + " - " + fileType);
                Console.WriteLine(newFileTitle);
                webClient.DownloadFile(fileTitle, newFileTitle);
                // Ejemplo de como se zipea un directorio 
                // System.IO.Compression.ZipFile.CreateFromDirectory(@"c:\example\start", @"D:\CompraNetTemporaryDataFiles\Contratos2018.zip");
                System.IO.Compression.ZipFile.ExtractToDirectory(newFileTitle, APFDataFiles);
            }
            conn.Close();

            try
            {
                string[] fileList = Directory.GetFiles(APFDataFiles, "*.xlsx");

                // List files.
                foreach (string f in fileList)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(APFDataFiles.Length + 1);
                    Console.WriteLine("Deleting - " + fName);
                    System.IO.File.Delete(f);
                }

                fileList = Directory.GetFiles(APFDataFiles, "*.*");

                // List files.
                foreach (string f in fileList)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(APFDataFiles.Length + 1);
                    Console.WriteLine("Deleting - " + fName);
                    System.IO.File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }

            Console.WriteLine("F I N");
            Console.ReadKey();
        }
    }
}

