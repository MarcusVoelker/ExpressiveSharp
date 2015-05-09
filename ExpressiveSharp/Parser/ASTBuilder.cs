using System;
using System.Collections.Generic;
using System.Linq;

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

                tokens.MoveNext();
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

            tokens.MoveNext();
            return idNode;
        }

        private static ASTNode BuildDot(IEnumerator<Token> tokens)
        {
            var lhs = BuildCore(tokens);
            while (tokens.Current.IsOperator(OperatorToken.OperatorType.Dot))
            {
                var op = tokens.Current;

                tokens.MoveNext();
                var rhs = BuildCore(tokens);

                var node = new ASTNode(op);
                node.Children.Add(lhs);
                node.Children.Add(rhs);
                lhs = node;
            }
            return lhs;
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
            while (tokens.Current.IsOperator(OperatorToken.OperatorType.Star) ||
                   tokens.Current.IsOperator(OperatorToken.OperatorType.Slash) ||
                   tokens.Current.IsOperator(OperatorToken.OperatorType.Percent))
            {
                var op = tokens.Current;

                tokens.MoveNext();
                var rhs = BuildNeg(tokens);

                var node = new ASTNode(op);
                node.Children.Add(lhs);
                node.Children.Add(rhs);
                lhs = node;
            }
            return lhs;
        }

        private static ASTNode BuildAdd(IEnumerator<Token> tokens)
        {
            var lhs = BuildMul(tokens);
            while (tokens.Current.IsOperator(OperatorToken.OperatorType.Plus) ||
                   tokens.Current.IsOperator(OperatorToken.OperatorType.Minus))
            {
                var op = tokens.Current;

                tokens.MoveNext();
                var rhs = BuildMul(tokens);

                var node = new ASTNode(op);
                node.Children.Add(lhs);
                node.Children.Add(rhs);
                lhs = node;
            }
            return lhs;
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

        private static ASTNode ReplaceInAst(Dictionary<string, KeyValuePair<ASTNode, ASTNode>> replacements, ASTNode ast)
        {
            for(var i = 0; i < ast.Children.Count; ++i)
                ast.Children[i] = ReplaceInAst(replacements, ast.Children[i]);

            var token = ast.Token as IdentifierToken;
            if (token == null || !replacements.ContainsKey(token.Name))
                return ast;

            var replacement = replacements[token.Name];
            var varMatches = ast.Match(replacement.Key);
            return varMatches == null ? ast : replacement.Value.Replace(varMatches);
        }

        private static ASTNode Merge(List<ASTNode> lines)
        {
            var replacements = new Dictionary<string, KeyValuePair<ASTNode,ASTNode>>();
            foreach (var line in lines)
            {
                if (!line.Token.IsOperator(OperatorToken.OperatorType.Equal))
                    return ReplaceInAst(replacements, line);

                var lhs = line.Children.First();
                if (!(lhs.Token is IdentifierToken))
                    throw new InvalidOperationException("Left-hand side of assignment " + line + " has to be named.");

                var name = ((IdentifierToken) lhs.Token).Name;
                replacements[name] = new KeyValuePair<ASTNode, ASTNode>(lhs, ReplaceInAst(replacements, line.Children.Last()));
            }
            throw new InvalidOperationException("No non-assignment line of code found.");
        }

        private static ASTNode Postprocess(ASTNode line)
        {
            for (var i = 0; i < line.Children.Count; ++i)
                line.Children[i] = Postprocess(line.Children[i]);

            if (line.Token.IsOperator(OperatorToken.OperatorType.Dot))
            {
                var node = line.Children.Last();
                node.Children.Insert(0,line.Children.First());
                return node;
            }
            return line;
        }

        public static AST BuildAst(IEnumerable<Token> tokens)
        {
            var enumerator = tokens.GetEnumerator(); //TODO multiline
            var lines = new List<ASTNode>();
            while (enumerator.MoveNext())
            {
                lines.Add(BuildLine(enumerator));
            }
            var singleLine = Postprocess(Merge(lines));
            return new AST(singleLine);
        }
    }
}
