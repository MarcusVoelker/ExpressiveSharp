using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ExpressiveSharp.Expression.Nodes;
using ExpressiveSharp.Expression.Nodes.Builtin;
using ExpressiveSharp.Parser;
using LLVMSharp;

namespace ExpressiveSharp.Expression
{
    public class Expression
    {
        private ExpressionNode rootNode;

        public delegate Tensor ExpressionFunction(Tensor[] args);
        public delegate void RawExpressionFunction(float[] args, float[] outs);

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

        public Expression(string code, Dictionary<string, TensorType> varTypes)
        {
            var ast = ASTBuilder.BuildAst(Tokenizer.Tokenize(code));
            rootNode = Translate(ast.Root);
            rootNode = rootNode.Preprocess(varTypes).FoldConstants();
        }

        public ExpressionFunction JITCompile()
        {
            var raw = rootNode.BuildLLVM();
            return args =>
            {
                var size = args.Sum(t => t.Type.ElementCount());
                var data = new float[size];
                var ctr = 0;
                foreach (var v in args.SelectMany(t => t.Data))
                    data[ctr++] = v;

                var outs = new float[rootNode.OutputType.ElementCount()];
                raw(data, outs);
                return new Tensor(rootNode.OutputType,outs);
            };
        }

        public RawExpressionFunction RawJITCompile()
        {
            return rootNode.BuildLLVM();
        }

        public override string ToString()
        {
            return rootNode.ToString();
        }
    }
}
