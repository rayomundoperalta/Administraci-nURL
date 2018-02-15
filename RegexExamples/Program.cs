using System;
using System.Text.RegularExpressions;

namespace RegexExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            string s1 = @"apple:green:3 banana:yellow:5";
            var myRegex = new Regex(@"(\w+):(\w+):(\d+)");

            ///////// The six main tasks we're likely to have ////////

            // Task 1: Is there a match?
            Console.WriteLine("*** Is there a Match? ***");
            if (myRegex.IsMatch(s1)) Console.WriteLine("Yes");
            else Console.WriteLine("No");

            // Task 2: How many matches are there?
            MatchCollection AllMatches = myRegex.Matches(s1);
            Console.WriteLine("\n" + "*** Number of Matches ***");
            Console.WriteLine(AllMatches.Count);

            // Task 3: What is the first match?
            Console.WriteLine("\n" + "*** First Match ***");
            Match OneMatch = myRegex.Match(s1);
            if (OneMatch.Success)
            {
                Console.WriteLine("Overall Match: " + OneMatch.Groups[0].Value);
                Console.WriteLine("Group 1: " + OneMatch.Groups[1].Value);
                Console.WriteLine("Group 2: " + OneMatch.Groups[2].Value);
                Console.WriteLine("Group 3: " + OneMatch.Groups[3].Value);
            }

            // Task 4: What are all the matches?
            Console.WriteLine("\n" + "*** Matches ***");
            if (AllMatches.Count > 0)
            {
                foreach (Match SomeMatch in AllMatches)
                {
                    Console.WriteLine("Overall Match: " + SomeMatch.Groups[0].Value);
                    Console.WriteLine("Group 1: " + SomeMatch.Groups[1].Value);
                    Console.WriteLine("Group 2: " + SomeMatch.Groups[2].Value);
                    Console.WriteLine("Group 3: " + SomeMatch.Groups[3].Value);
                }
            }

            // Task 5: Replace the matches
            // simple replacement: reverse groups
            string replaced = myRegex.Replace(s1,
                   delegate (Match m) {
                       return m.Groups[3].Value + ":" +
                      m.Groups[2].Value + ":" +
                      m.Groups[1].Value;
                   }
                    );
            Console.WriteLine("\n" + "*** Replacements ***");
            Console.WriteLine("Origen: " + s1);
            Console.WriteLine("Expresion Regular: " + @"(\w+):(\w+):(\d+)");
            Console.WriteLine(replaced);

            // Task 6: Split
            // Let's split at colons or spaces
            string[] splits = Regex.Split(s1, @":|\s");
            Console.WriteLine("\n" + "*** Splits ***");
            foreach (string split in splits) Console.WriteLine(split);

            Console.WriteLine("\nPress Any Key to Exit.");
            Console.ReadKey();

        }
    }
}
