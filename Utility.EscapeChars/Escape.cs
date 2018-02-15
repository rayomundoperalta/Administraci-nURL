using System.Text.RegularExpressions;

namespace Utility.Ecape
{
    public class Escape
    {
        Regex myRegex;

        public Escape(string RegExp)
        {
            myRegex = new Regex(RegExp);
        }

        public string Reemplaza(string input)
        {
            return myRegex.Replace(input,
                   delegate (Match m) {
                       return "'" + m.Groups[1].Value;
                   });
        }
    }
}
