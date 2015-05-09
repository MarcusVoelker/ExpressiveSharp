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
            var f = exp.JITCompile();
            Assert.AreEqual(0, f(new Tensor[] { 0 })[0]);
            Assert.AreEqual(20, f(new Tensor[] { 1 })[0]);
            Assert.AreEqual(40, f(new Tensor[] { 2 })[0]);
            Assert.AreEqual(60, f(new Tensor[] { 3 })[0]);
            Assert.AreEqual(80, f(new Tensor[] { 4 })[0]);
        }
    }
}
