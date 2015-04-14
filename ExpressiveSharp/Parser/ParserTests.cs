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
            var tokens = Tokenizer.Tokenize("1.2.abc12.12b");
            Assert.True(tokens.SequenceEqual(new Token[]
            {
                new ConstantToken(1.2f),
                new OperatorToken(OperatorToken.OperatorType.Dot),
                new IdentifierToken("abc12"),
                new ConstantToken(.12f),
                new IdentifierToken("b"),
            }));
        }
    }
}
