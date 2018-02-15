using System;
using System.Text.RegularExpressions;

namespace RegExPatternMatching
{
    class Program
    {
        static void Main(string[] args)
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
            Console.WriteLine("F I N");
            Console.ReadKey();
        }
    }
}




