﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace TipoContrato
{
    // Tokens that represent the input
    internal enum Token
    {
        AñoSimple,
        AñoCompuesto,
        Other       // Represents unrecognized charachters
    }

    // Scanner used to find the tokens from a calc lampda string expression
    internal static class LambdaCalcScanner
    {
        // The pattern used with the regular expression class to scan the input
        const string Pattern = @"
                (?'AñoSimple' contratos_20\d{2}_\d{14}.xlsx ) |
                (?'AñoCompuesto' contratos_20\d{2}_20\d{2}_\d{14}.xlsx ) |
                (?'Other' [^ \r\n\t])";

        // Regular expression used to scan the input
        private static Regex MathRegex = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        // Enumurable to get tokens from the given expression (scanner)
        public static IEnumerable<TokenEntity> GetLambdaCalcTokens(this string exp)
        {
            Token[] tokens = Enum.GetValues(typeof(Token)).OfType<Token>().ToArray();

            foreach (Match m in MathRegex.Matches(exp))
            {
                foreach (Group group in m.Groups)
                {
                    Console.WriteLine("-G> " + group.Value);
                }
                // Check which token is matched by this match object
                foreach (Token token in tokens)
                {
                    Console.Write(">TS> " + token.ToString() + " - " + m.Groups[token.ToString()].Value);
                    if (m.Groups[token.ToString()].Success)
                    {
                        Console.WriteLine(m.Index.ToString() + " " + m.Value);
                        yield return new TokenEntity(
                            token,
                            m.Index,
                            m.Value);
                    }
                }
            }
            // return the end string token, to indecate we are done
            yield return new TokenEntity(Token.Other, exp.Length, "\0");
        }

        public static IEnumerable<TokenEntity> NoEleganteGetLambdaCalcTokens(string exp)
        {
            Token[] tokens = Enum.GetValues(typeof(Token)).OfType<Token>().ToArray();
            foreach (Match m in MathRegex.Matches(exp))
            {
                // Check which token is matched by this match object
                foreach (Token token in tokens)
                {
                    if (m.Groups[token.ToString()].Success)
                    {
                        yield return new TokenEntity(
                            token,
                            m.Index,
                            m.Value);
                    }
                }
            }
            // return the end string token, to indecate we are done
            yield return new TokenEntity(Token.Other, exp.Length, "\0");
        }
    }

    // Holds token info
    internal class TokenEntity
    {
        public TokenEntity(Token token, int startPos, string value)
        {
            this.Token = token;
            this.StartPos = startPos;
            this.Value = value;
        }

        // Token type
        public Token Token { get; private set; }

        // Start position in the original string
        public int StartPos { get; private set; }

        // Value
        public string Value { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} at {1}: {2}", Token, StartPos, Value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            foreach (TokenEntity t in "contratos_2010_2012_20180215142534.xlsx".GetLambdaCalcTokens())
            {
                Console.WriteLine(t);
            }

            foreach (TokenEntity t in "contratos_2013_20180214163710.xlsx".GetLambdaCalcTokens())
            {
                Console.WriteLine(t);
            }

            foreach (TokenEntity t in LambdaCalcScanner.NoEleganteGetLambdaCalcTokens("contratos_2010_2012_20180215142534.xlsx"))
            {
                Console.WriteLine(t);
            }

            foreach (TokenEntity t in LambdaCalcScanner.NoEleganteGetLambdaCalcTokens("contratos_2013_20180214163710.xlsx"))
            {
                Console.WriteLine(t);
            }

            Console.WriteLine("F I N");
            Console.ReadKey();
        }
    }
}
