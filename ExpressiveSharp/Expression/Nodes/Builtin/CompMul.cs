using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class CompMul : FunctionNode
    {
        public override string FunctionName => "compMul";

        public override TypeClass TypeClass => TypeClass.TensorTensorTensor;

        public CompMul(IEnumerable<ExpressionNode> children) : base(children)
        {
        }
    }
}
