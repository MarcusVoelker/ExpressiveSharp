using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
        public void JittingTest1()
        {
            var exp = new Expression("x*5*4", new Dictionary<string, TensorType>
            {
                {"x", new TensorType()}
            });
            var f = exp.JITCompile();
            Assert.AreEqual(0, f(new Tensor[] {0})[0]);
            Assert.AreEqual(20, f(new Tensor[] {1})[0]);
            Assert.AreEqual(40, f(new Tensor[] {2})[0]);
            Assert.AreEqual(60, f(new Tensor[] {3})[0]);
            Assert.AreEqual(80, f(new Tensor[] {4})[0]);
        }

        [Test]
        public void JittingTest2()
        {

            var exp = new Expression("x+y", new Dictionary<string, TensorType>
            {
                {"x", new TensorType(3)},
                {"y", new TensorType()}
            });
            var f = exp.RawJITCompile();
            float[] ins = new float[4];
            float[] outs = new float[3];
            ins[0] = 1;
            ins[1] = 2;
            ins[2] = 3;
            ins[3] = 1;
            f(ins, outs);
            Assert.AreEqual(2, outs[0]);
            Assert.AreEqual(3, outs[1]);
            Assert.AreEqual(4, outs[2]);
        }
    }
}
