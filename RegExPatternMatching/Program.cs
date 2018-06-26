using System;
using System.Text.RegularExpressions;

namespace RegExPatternMatching
{
    class Program
    {
        public static void EjemploGrupos()
        {
            string input = "This is the first sentence. Is it the beginning " +
                           "of a literary masterpiece? I think not. Instead, " +
                           "it is a nonsensical paragraph.";
            string pattern = @"\b\(?((?>\w+),?\s?)+[\.!?]\)?";
            Console.WriteLine("With implicit captures:");
            foreach (Match match in Regex.Matches(input, pattern))
            {
                Console.WriteLine("The match: {0}", match.Value);
                int groupCtr = 0;
                foreach (Group group in match.Groups)
                {
                    Console.WriteLine("   Group {0}: {1}", groupCtr, group.Value);
                    groupCtr++;
                    int captureCtr = 0;
                    foreach (Capture capture in group.Captures)
                    {
                        Console.WriteLine("      Capture {0}: {1}", captureCtr, capture.Value);
                        captureCtr++;
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("With explicit captures only:");
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.ExplicitCapture))
            {
                Console.WriteLine("The match: {0}", match.Value);
                int groupCtr = 0;
                foreach (Group group in match.Groups)
                {
                    Console.WriteLine("   Group {0}: {1}", groupCtr, group.Value);
                    groupCtr++;
                    int captureCtr = 0;
                    foreach (Capture capture in group.Captures)
                    {
                        Console.WriteLine("      Capture {0}: {1}", captureCtr, capture.Value);
                        captureCtr++;
                    }
                }
            }
        }

        public static void SegundoEjemploGrupos()
        {
            string input = "This is the first sentence. Is it the beginning " +
                           "of a literary masterpiece? I think not. Instead, " +
                           "it is a nonsensical paragraph pentium-586 i7 1525.";
            string pattern = @"\b\(?((?>\w+),?\s?)+[\.!?]\)?";
            Console.WriteLine("With implicit captures:");
            foreach (Match match in Regex.Matches(input, pattern))
            {
                Console.WriteLine("The match: {0}", match.Value);
                int groupCtr = 0;
                foreach (Group group in match.Groups)
                {
                    Console.WriteLine("   Group {0}: {1}", groupCtr, group.Value);
                    groupCtr++;
                    int captureCtr = 0;
                    foreach (Capture capture in group.Captures)
                    {
                        Console.WriteLine("      Capture {0}: {1}", captureCtr, capture.Value);
                        captureCtr++;
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("With explicit captures only:");
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.ExplicitCapture))
            {
                Console.WriteLine("The match: {0}", match.Value);
                int groupCtr = 0;
                foreach (Group group in match.Groups)
                {
                    Console.WriteLine("   Group {0}: {1}", groupCtr, group.Value);
                    groupCtr++;
                    int captureCtr = 0;
                    foreach (Capture capture in group.Captures)
                    {
                        Console.WriteLine("      Capture {0}: {1}", captureCtr, capture.Value);
                        captureCtr++;
                    }
                }
            }
        }

        static void EjemploAlternation()
        {
            // Regular expression using character class.
            string pattern1 = @"\bgr[ae]y\b";
            // Regular expression using either/or.
            string pattern2 = @"\bgr(a|e)y\b";

            string input = "The gray wolf blended in among the grey rocks.";
            foreach (Match match in Regex.Matches(input, pattern1))
                Console.WriteLine("'{0}' found at position {1}",
                                  match.Value, match.Index);
            Console.WriteLine();
            foreach (Match match in Regex.Matches(input, pattern2))
                Console.WriteLine("'{0}' found at position {1}",
                                  match.Value, match.Index);
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("Ejemplo Alternation\n");
            //EjemploAlternation();
            //Console.WriteLine("Ejemplo Groups\n");
            //EjemploGrupos();
            SegundoEjemploGrupos();
            Console.WriteLine("F I N");
            Console.ReadKey();
        }
    }
}




