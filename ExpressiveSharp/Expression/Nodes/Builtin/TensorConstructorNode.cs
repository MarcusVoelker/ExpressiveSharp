using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class TensorConstructorNode : BuiltinNode
    {
        public override string FunctionName => "tensor";

        public override TypeClass TypeClass => TypeClass.Complex;

        public TensorConstructorNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }

        public TensorConstructorNode(params ExpressionNode[] children) : base(children)
        {
        }
    }
}
