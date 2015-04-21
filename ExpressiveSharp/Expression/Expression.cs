using System;
using ExpressiveSharp.Expression.Nodes;
using ExpressiveSharp.Parser;

namespace ExpressiveSharp.Expression
{
    public class Expression
    {
        private ExpressionNode rootNode;

        private Expression(ExpressionNode rootNode)
        {
            this.rootNode = rootNode;
        }

        private static ExpressionNode Translate(ASTNode node)
        {
            {
                var token = node.Token as ConstantToken;
                if (token != null)
                    return new ConstantNode(token.Value);
            }
            {
                var token = node.Token as IdentifierToken;
                if (token != null)
                {
                    if (node.Children.Count == 0)
                        return new VariableNode(token.Name);

                }
            }
            {
                var token = node.Token as OperatorToken;
                if (token != null)
                {
                }
            }
            return null;
        }

        public Expression(string code)
        {
            var ast = ASTBuilder.BuildAst(Tokenizer.Tokenize(code));

        }
    }
}
