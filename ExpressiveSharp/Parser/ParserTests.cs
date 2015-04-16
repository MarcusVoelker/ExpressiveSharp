using System;
using System.Linq;
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
        }
    }
}
