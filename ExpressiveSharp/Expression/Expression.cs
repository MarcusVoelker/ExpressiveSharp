using System;
using System.Collections.Generic;
using System.Linq;
using ExpressiveSharp.Expression.Nodes;
using ExpressiveSharp.Expression.Nodes.Builtin;
using ExpressiveSharp.Parser;
using LLVMSharp;

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
                var children = node.Children.Select(Translate);
                var token = node.Token as OperatorToken;
                if (token != null)
                {
                    switch (token.Type)
                    {
                        case OperatorToken.OperatorType.Dot:
                        case OperatorToken.OperatorType.LParen:
                        case OperatorToken.OperatorType.RParen:
                        case OperatorToken.OperatorType.Comma:
                        case OperatorToken.OperatorType.Semicolon:
                        case OperatorToken.OperatorType.Equal:
                            throw new InvalidOperationException("Stray operator " + token);
                        case OperatorToken.OperatorType.Plus:
                            return new AddNode(children.ToList());
                        case OperatorToken.OperatorType.Minus:
                            return new SubNode(children.ToList());
                        case OperatorToken.OperatorType.Star:
                            return new MulNode(children.ToList());
                        case OperatorToken.OperatorType.Slash:
                            return new DivNode(children.ToList());
                        case OperatorToken.OperatorType.Percent:
                            return new ModNode(children.ToList());
                        default:
                            throw new ArgumentOutOfRangeException("Unhandled operator " + token);
                    }
                }
            }
            return null;
        }

        public Expression(string code, Dictionary<string,TensorType> varTypes)
        {
            var ast = ASTBuilder.BuildAst(Tokenizer.Tokenize(code));
            rootNode = Translate(ast.Root);
            rootNode = rootNode.Preprocess(varTypes).FoldConstants();
        }

        public void JITCompile()
        {
            foreach (var v in rootNode.BuildLLVM())
                LLVM.DumpValue(v);
        }

        public override string ToString()
        {
            return rootNode.ToString();
        }
    }
}
