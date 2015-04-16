﻿using System;
using System.Collections.Generic;

namespace ExpressiveSharp.Parser
{
    internal static class ASTBuilder
    {
        private static ASTNode BuildCore(IEnumerator<Token> tokens)
        {
            if (tokens.Current.IsOperator(OperatorToken.OperatorType.LParen))
            {
                tokens.MoveNext();
                var inner = BuildAdd(tokens);
                if (!tokens.Current.IsOperator(OperatorToken.OperatorType.RParen))
                    throw new InvalidOperationException("Expected RParen, but got " + tokens.Current);

                return inner;
            }

            if (tokens.Current is ConstantToken)
            {
                var cst = tokens.Current;
                tokens.MoveNext();
                return new ASTNode(cst);
            }

            if (!(tokens.Current is IdentifierToken))
                throw new InvalidOperationException("Expected Identifier, but got " + tokens.Current);

            var id = (IdentifierToken) tokens.Current;
            var idNode = new ASTNode(id);
            tokens.MoveNext();
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.LParen))
                return idNode;
            
            tokens.MoveNext();
            if (tokens.Current.IsOperator(OperatorToken.OperatorType.RParen))
                return idNode;

            idNode.Children.Add(BuildAdd(tokens));
            while (tokens.Current.IsOperator(OperatorToken.OperatorType.Comma))
            {
                tokens.MoveNext();
                idNode.Children.Add(BuildAdd(tokens));
            }
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.RParen))
                throw new InvalidOperationException("Expected RParen, but got " + tokens.Current);

            return idNode;
        }

        private static ASTNode BuildDot(IEnumerator<Token> tokens)
        {
            var lhs = BuildCore(tokens);
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.Dot))
                return lhs;

            var op = tokens.Current;

            tokens.MoveNext();
            var rhs = BuildCore(tokens);

            var node = new ASTNode(op);
            node.Children.Add(lhs);
            node.Children.Add(rhs);
            return node;
        }

        private static ASTNode BuildNeg(IEnumerator<Token> tokens)
        {
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.Minus))
                return BuildDot(tokens);

            var op = tokens.Current;
            tokens.MoveNext();

            var node = new ASTNode(op);
            node.Children.Add(BuildDot(tokens));
            return node;
        }

        private static ASTNode BuildMul(IEnumerator<Token> tokens)
        {
            var lhs = BuildNeg(tokens);
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.Star) &&
                !tokens.Current.IsOperator(OperatorToken.OperatorType.Slash) &&
                !tokens.Current.IsOperator(OperatorToken.OperatorType.Percent))
                return lhs;

            var op = tokens.Current;

            tokens.MoveNext();
            var rhs = BuildMul(tokens);

            var node = new ASTNode(op);
            node.Children.Add(lhs);
            node.Children.Add(rhs);
            return node;
        }

        private static ASTNode BuildAdd(IEnumerator<Token> tokens)
        {
            var lhs = BuildMul(tokens);
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.Plus) &&
                !tokens.Current.IsOperator(OperatorToken.OperatorType.Minus))
                return lhs;

            var op = tokens.Current;

            tokens.MoveNext();
            var rhs = BuildAdd(tokens);

            var node = new ASTNode(op);
            node.Children.Add(lhs);
            node.Children.Add(rhs);
            return node;
        }

        private static ASTNode BuildLine(IEnumerator<Token> tokens)
        {
            var lhs = BuildAdd(tokens);
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.Equal))
                return lhs;

            var eq = tokens.Current;

            tokens.MoveNext();
            var rhs = BuildAdd(tokens);
            if (!tokens.Current.IsOperator(OperatorToken.OperatorType.Semicolon))
                throw new InvalidOperationException("Expected Semicolon, got" + tokens.Current);

            var node = new ASTNode(eq);
            node.Children.Add(lhs);
            node.Children.Add(rhs);
            return node;
        }

        public static AST BuildAst(IEnumerable<Token> tokens)
        {
            var enumerator = tokens.GetEnumerator(); //TODO multiline
            enumerator.MoveNext();
            return new AST(BuildLine(enumerator));
        }
    }
}
