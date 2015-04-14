using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Parser
{
    static class Tokenizer
    {
        private enum State
        {
            NextToken,
            Constant,
            ConstantPostDot,
            Dot,
            Identifier,
        }

        private static Token Constant(string code)
        {
            return new ConstantToken(float.Parse(code,CultureInfo.InvariantCulture.NumberFormat));
        }

        private static Token Identifier(string code)
        {
            return new IdentifierToken(code);
        }

        private static Token Operator(string code)
        {
            switch (code)
            {
                case ".":
                    return new OperatorToken(OperatorToken.OperatorType.Dot);
            }
            throw new InvalidOperationException("Unknown operator " + code);
        }

        public static IEnumerable<Token> Tokenize(string code)
        {
            var state = State.NextToken;
            string accumulator = "";
            foreach (var c in code)
            {
                if (char.IsDigit(c))
                {
                    switch (state)
                    {
                        case State.NextToken:
                            state = State.Constant;
                            accumulator = "" + c;
                            break;
                        case State.Constant:
                            accumulator += c;
                            break;
                        case State.ConstantPostDot:
                            accumulator += c;
                            break;
                        case State.Dot:
                            state = State.ConstantPostDot;
                            accumulator += c;
                            break;
                        case State.Identifier:
                            accumulator += c;
                            break;
                    }
                    continue;
                }
                if (c == '.')
                {
                    switch (state)
                    {
                        case State.NextToken:
                            state = State.Dot;
                            accumulator = ".";
                            break;
                        case State.Constant:
                            state = State.ConstantPostDot;
                            accumulator += c;
                            break;
                        case State.ConstantPostDot:
                            state = State.Dot;
                            yield return Constant(accumulator);
                            accumulator = ".";
                            break;
                        case State.Dot:
                            throw new InvalidOperationException("Code contains two consecutive dots");
                        case State.Identifier:
                            yield return Identifier(accumulator);
                            state = State.Dot;
                            accumulator = ".";
                            break;
                    }
                    continue;
                }
                if (char.IsLetter(c))
                {
                    switch (state)
                    {
                        case State.NextToken:
                            state = State.Identifier;
                            accumulator = "" + c;
                            break;
                        case State.Constant:
                        case State.ConstantPostDot:
                            yield return Constant(accumulator);
                            state = State.Identifier;
                            accumulator = "" + c;
                            break;
                        case State.Dot:
                            yield return Operator(accumulator);
                            state = State.Identifier;
                            accumulator = "" + c;
                            break;
                        case State.Identifier:
                            accumulator += c;
                            break;
                    }
                    continue;
                }
                if (char.IsWhiteSpace(c))
                {
                    switch (state)
                    {
                        case State.NextToken:
                            break;
                        case State.Constant:
                        case State.ConstantPostDot:
                            yield return Constant(accumulator);
                            break;
                        case State.Dot:
                            throw new InvalidOperationException("Code ends on dot");
                        case State.Identifier:
                            yield return Identifier(accumulator);
                            break;
                    }
                    state = State.NextToken;
                    continue;
                }
            } 
            switch (state)
            {
                case State.NextToken:
                    break;
                case State.Constant:
                case State.ConstantPostDot:
                    yield return Constant(accumulator);
                    break;
                case State.Dot:
                    throw new InvalidOperationException("Code ends on dot");
                case State.Identifier:
                    yield return Identifier(accumulator);
                    break;
            }
        }

    }
}
