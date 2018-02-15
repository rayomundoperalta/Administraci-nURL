using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace LambdaCalc
{
    // Tokens that represent the input
    internal enum Token
    {
        OpenParan, CloseParan,
        Arrow,
        Comma,
        Plus, Minus, Multiply, Divide,
        Constant,
        Variable,
        Other       // Represents unrecognized charachters
    }

    // Scanner used to find the tokens from a calc lampda string expression
    internal static class LambdaCalcScanner
    {
        // The pattern used with the regular expression class to scan the input
        const string Pattern = @"
                (?'OpenParan' \( ) | (?'CloseParan' \) ) |
                (?'Arrow' => ) |
                (?'Comma' ,  ) |
                (?'Plus' \+ ) | (?'Minus' - ) | (?'Multiply' \* ) | (?'Divide' / ) |
                (?'Constant' (\.\d+|\d+(\.\d+)?) ) |
                (?'Variable' [a-zA-Z]\w* ) |
                (?'Other' [^ \r\n\t])";

        // Regular expression used to scan the input
        private static Regex MathRegex = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        // Enumurable to get tokens from the given expression (scanner)
        public static IEnumerable<TokenEntity> GetLambdaCalcTokens(this string exp)
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

    /// <summary>
    /// Test program
    /// </summary>
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        foreach (TokenEntity t in "(a, b) => ; a + b * 0.1".GetLambdaCalcTokens())
    //        {
    //            Console.WriteLine(t);
    //        }
    //    }
    //}

    /// <summary>
    /// Lambda expression calculation class
    /// </summary>
    public class LambdaCalc
    {
        // Delegate that holds the compiled lampda expression
        public delegate double Func(params double[] args);

        TokenEntity lastToken;
        // Holds the tokens enumurator
        IEnumerator<TokenEntity> tokens;
        // Holds the list of parameters in the math equation
        List<string> parameters;
        // Holds the lampda expression parameter, this is the params double [] args
        ParameterExpression funcParamsArg = Expression.Parameter(typeof(double[]), "args");
        // Holds the parsed lambda expression
        Expression<Func> lambdaExp;
        // Holds the compiled delegate 
        Func function;

        /// <summary>
        /// Initialize the class from the given string expression
        /// </summary>
        /// <param name="exp">Input expression, ie: (a, b) => a + b * 2</param>
        public LambdaCalc(string exp)
        {
            this.lambdaExp = ParseExpression(exp);
        }

        /// <summary>
        /// Gets the compiled delegate of the input expression
        /// </summary>
        public Func Function
        {
            get
            {
                if (this.function == null)
                {
                    this.function = this.lambdaExp.Compile();
                }
                return this.function;
            }
        }

        private TokenEntity CurrentToken { get { return this.tokens.Current; } }
        private TokenEntity LastToken { get { return this.lastToken; } }

        private bool AdvanceToken()
        {
            this.lastToken = CurrentToken;
            return this.tokens.MoveNext();
        }

        private bool CheckToken(Token token)
        {
            if (CurrentToken != null && CurrentToken.Token == token)
            {
                AdvanceToken();
                return true;
            }
            return false;
        }

        // Parse the given expression, expect ([[[<param1>],<param2>],..]) => <body>
        Expression<Func> ParseExpression(string exp)
        {
            this.tokens = exp.GetLambdaCalcTokens().GetEnumerator();
            AdvanceToken();

            ParseParameters();

            if (!CheckToken(Token.Arrow))
                throw GetErrorException("Expecting equation arrow.", CurrentToken);

            Expression body = BodyExpression();

            if (CurrentToken.Value != "\0")   // check if we didn't reach the end
                throw GetErrorException("Invalid equation syntax.", CurrentToken);

            Expression<Func> lambdaExpr = Expression.Lambda<Func>(body, this.funcParamsArg);
            Console.WriteLine("Generated lambda expr: " + lambdaExpr.ToString());
            return lambdaExpr;
        }

        // Parse the parameters list (a, b, ...)
        private void ParseParameters()
        {
            this.parameters = new List<string>();

            if (!CheckToken(Token.OpenParan))
                throw GetErrorException("Expecting open paran.", CurrentToken);

            if (!CheckToken(Token.CloseParan))
            {
                do
                {
                    if (!CheckToken(Token.Variable))
                        throw GetErrorException("Expecting a parameter.", CurrentToken);
                    // add parameter
                    this.parameters.Add(LastToken.Value.ToLower());
                } while (CheckToken(Token.Comma));
            }
            if (!CheckToken(Token.CloseParan))
                throw GetErrorException("Expecting close paran.", CurrentToken);
        }

        // Get a literal expression if found
        private Expression Literal()
        {
            Expression literal = null;
            if (CheckToken(Token.Constant))
            {
                literal = Expression.Constant(double.Parse(LastToken.Value));
            }
            return literal;
        }

        // Get an identifier reference expression if found
        private Expression Identifier()
        {
            Expression identifier = null;
            if (CheckToken(Token.Variable))
            {
                int parameterIndex = this.parameters.IndexOf(LastToken.Value.ToLower());
                if (parameterIndex < 0)
                    throw GetErrorException("Invalid reference to '" + LastToken.Value + "'.", LastToken);
                identifier = Expression.ArrayIndex(this.funcParamsArg, Expression.Constant(parameterIndex));
            }
            return identifier;
        }

        // Parse a paranthasis expression (...)
        private Expression Paran()
        {
            Expression paran = null;
            if (CheckToken(Token.OpenParan))
            {
                paran = BodyExpression();
                if (!CheckToken(Token.CloseParan))
                    throw GetErrorException("A close paranthasis is missing.", LastToken);
            }
            return paran;
        }

        // Parse and returns the constant or variable expression
        private Expression PrimaryExpression()
        {
            Expression primaryExpression;
            if ((primaryExpression = Literal()) != null ||
                (primaryExpression = Identifier()) != null ||
                (primaryExpression = Paran()) != null)
            {
                return primaryExpression;
            }
            throw GetErrorException("Expecting a constant or parameter reference.", CurrentToken);
        }

        // Parse and returns the unary expression
        private Expression UnaryExpression()
        {
            if (CheckToken(Token.Plus) || CheckToken(Token.Minus))
            {
                if (LastToken.Token == Token.Minus)
                    return Expression.Negate(UnaryExpression());
                else
                    return UnaryExpression();
            }
            return PrimaryExpression();
        }

        // Parse and returns the multiplication/division expression 
        private Expression MultiplicativeExpression()
        {
            Expression multiplicativeExpr = UnaryExpression();
            while (CheckToken(Token.Multiply) || CheckToken(Token.Divide))
            {
                if (LastToken.Token == Token.Multiply)
                    multiplicativeExpr = Expression.Multiply(multiplicativeExpr, UnaryExpression());
                else if (LastToken.Token == Token.Multiply)
                    multiplicativeExpr = Expression.Divide(multiplicativeExpr, UnaryExpression());
            }
            return multiplicativeExpr;
        }

        // Parse and returns the addition/subtraction expression 
        private Expression AdditiveExpression()
        {
            Expression additionExpr = MultiplicativeExpression();
            while (CheckToken(Token.Plus) || CheckToken(Token.Minus))
            {
                if (LastToken.Token == Token.Plus)
                    additionExpr = Expression.Add(additionExpr, MultiplicativeExpression());
                else if (LastToken.Token == Token.Minus)
                    additionExpr = Expression.Subtract(additionExpr, MultiplicativeExpression());
            }
            return additionExpr;
        }

        // Parse and returns the body expression
        private Expression BodyExpression()
        {
            return AdditiveExpression();
        }

        // Returns back an error exception
        private Exception GetErrorException(string p, TokenEntity tokenEntity)
        {
            return new Exception(string.Format("Error at '{0}': {1}", tokenEntity != null ? tokenEntity.StartPos : 0, p));
        }
    }

    /// <summary>
    /// Test program
    /// </summary>
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        LambdaCalc lc = new LambdaCalc("(a, b) => -a + 2 * b");
    //        Console.WriteLine(lc.Function(1, 2));
    //    }
    //}

    class Program
    {
        static void Main(string[] args)
        {
            LambdaCalc lc = new LambdaCalc("(a, b) => -a + 2 * b");
            Console.WriteLine(lc.Function(80, 2));
            Console.ReadKey();
        }
    }
}
