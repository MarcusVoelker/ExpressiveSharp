using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ExpressiveSharp.Expression
{
    [TestFixture]
    class ExpressionTests
    {
        [Test]
        public void JittingTest()
        {
            var exp = new Expression("x*5*4", new Dictionary<string, TensorType>
            {
                {"x", new TensorType()}
            });
            Console.WriteLine(exp);
            exp.JITCompile();
        }
    }
}
