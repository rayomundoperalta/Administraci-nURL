using System;
using Utility.Ecape;

namespace RegexExamples
{
    class Program
    {
        

        static void Main(string[] args)
        {
            string s1 = @"ADNS-01-10-11,,,,'' MANTENIMIENTO A DVR'S";
            string RegExp = @"(['])";
            Escape escapa = new Escape(RegExp);

            string replaced = escapa.Reemplaza(s1);
           
            Console.WriteLine("\n" + "*** Replacements ***");
            Console.WriteLine("Origen: " + s1);
            Console.WriteLine("Expresion Regular: " + RegExp);
            Console.WriteLine(replaced);

            Console.WriteLine("\nPress Any Key to Exit.");
            Console.ReadKey();

        }
    }
}
