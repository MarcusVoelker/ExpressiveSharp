using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class DivNode : BuiltinNode
    {
        public override string FunctionName => "div";

        public override TypeClass TypeClass => TypeClass.TensorTensorTensor;

        public override string ToString()
        {
            return "(" + Children.First() + " / " + Children.Last() + ")";
        }

        public DivNode(IEnumerable<ExpressionNode> children) : base(children)
        {

        }
    }
}
