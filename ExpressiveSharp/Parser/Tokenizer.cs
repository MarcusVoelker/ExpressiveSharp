using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExpressiveSharp.Parser
{
    internal static class Tokenizer
    {
        private enum State
        {
            NextToken,
            Constant,
            ConstantDot,
            ConstantPostDot,
            Dot,
            Identifier,
            Operator,
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
                case "(":
                    return new OperatorToken(OperatorToken.OperatorType.LParen);
                case ")":
                    return new OperatorToken(OperatorToken.OperatorType.RParen);
                case "+":
                    return new OperatorToken(OperatorToken.OperatorType.Plus);
                case "-":
                    return new OperatorToken(OperatorToken.OperatorType.Minus);
                case "*":
                    return new OperatorToken(OperatorToken.OperatorType.Star);
                case "/":
                    return new OperatorToken(OperatorToken.OperatorType.Slash);
                case "%":
                    return new OperatorToken(OperatorToken.OperatorType.Percent);
                case "=":
                    return new OperatorToken(OperatorToken.OperatorType.Equal);
                case ",":
                    return new OperatorToken(OperatorToken.OperatorType.Comma);
                case ";":
                    return new OperatorToken(OperatorToken.OperatorType.Semicolon);
            }
            throw new InvalidOperationException("Unknown operator " + code);
        }

        public static IEnumerable<Token> Tokenize(string code)
        {
            var state = State.NextToken;
            var accumulator = "";
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
                        case State.ConstantDot:
                            state = State.ConstantPostDot;
                            accumulator += "." + c;
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
                        case State.Operator:
                            state = State.Constant;
                            yield return Operator(accumulator);
                            accumulator = "" + c;
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
                            state = State.ConstantDot;
                            break;
                        case State.ConstantDot:
                            throw new InvalidOperationException("Code contains two consecutive dots");
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
                        case State.Operator:
                            state = State.Dot;
                            yield return Operator(accumulator);
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
                        case State.ConstantDot:
                            yield return Constant(accumulator);
                            yield return Operator(".");
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
                        case State.Operator:
                            state = State.Identifier;
                            yield return Operator(accumulator);
                            accumulator = "" + c;
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
                        case State.ConstantDot:
                            yield return Constant(accumulator);
                            yield return Operator(".");
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
                        case State.Operator:
                            yield return Operator(accumulator);
                            break;
                    }
                    state = State.NextToken;
                    continue;
                }
                //Operator
                switch (state)
                {
                    case State.NextToken:
                        accumulator = "" + c;
                        state = State.Operator;
                        break;
                    case State.ConstantDot:
                        yield return Constant(accumulator);
                        yield return Operator(".");
                        break;
                    case State.Constant:
                    case State.ConstantPostDot:
                        yield return Constant(accumulator);
                        accumulator = "" + c;
                        state = State.Operator;
                        break;
                    case State.Dot:
                        yield return Operator(accumulator);
                        accumulator = "" + c;
                        state = State.Operator;
                        break;
                    case State.Identifier:
                        yield return Identifier(accumulator);
                        accumulator = "" + c;
                        state = State.Operator;
                        break;
                    case State.Operator:
                        yield return Operator(accumulator);
                        accumulator = "" + c;
                        break;
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
                case State.Operator:
                    yield return Operator(accumulator);
                    break;
            }
        }

    }
}
