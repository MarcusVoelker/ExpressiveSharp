using System;
using System.Collections.Generic;
using System.Linq;
using ExpressiveSharp.Expression;
using NUnit.Framework;

namespace ExpressiveSharp.Parser
{
    [TestFixture]
    class ParserTests
    {
        [Test]
        public void TokenizerTest()
        {
            var tokens = Tokenizer.Tokenize("1.2.abc12.12b+-.2.b*");
            Assert.True(tokens.SequenceEqual(new Token[]
            {
                new ConstantToken(1.2f),
                new OperatorToken(OperatorToken.OperatorType.Dot),
                new IdentifierToken("abc12"),
                new ConstantToken(.12f),
                new IdentifierToken("b"),
                new OperatorToken(OperatorToken.OperatorType.Plus),
                new OperatorToken(OperatorToken.OperatorType.Minus),
                new ConstantToken(.2f),
                new OperatorToken(OperatorToken.OperatorType.Dot),
                new IdentifierToken("b"),
                new OperatorToken(OperatorToken.OperatorType.Star),
            }));
        }

        [Test]
        public void AstBuilderTest()
        {
            var ast = ASTBuilder.BuildAst(Tokenizer.Tokenize("x*4+3.vec3(2,1)"));
            Console.WriteLine(ast.ToString());
            var ast2 = ASTBuilder.BuildAst(Tokenizer.Tokenize("x = vec2(1,2); y = 3; x.func(y * x)"));
            Console.WriteLine(ast2.ToString());
            var ast3 = ASTBuilder.BuildAst(Tokenizer.Tokenize("f(a,b,c) = a+b*c; g(a) = f(f(a-1,a-2,a-3),f(a-4,a-5,a-6),f(a-7,a-8,a-9)); g(5)"));
            Console.WriteLine(ast3.ToString());
        }

        [Test]
        public void ExpressionBuildingTest()
        {
            var exp = new Expression.Expression("x*4+(y-1)%4", new Dictionary<string, TensorType>
            {
                {"x", new TensorType()},
                {"y", new TensorType(3)}
            });
            Console.WriteLine(exp);
        }

        [Test]
        public void JittingTest()
        {
            var exp = new Expression.Expression("x*4+(y-1)", new Dictionary<string, TensorType>
            {
                {"x", new TensorType()},
                {"y", new TensorType()}
            });
            Console.WriteLine(exp);
            exp.JITCompile();
        }
    }
}
