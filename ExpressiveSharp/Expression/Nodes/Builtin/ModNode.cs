using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class ModNode : FunctionNode
    {
        public override string FunctionName => "mod";

        public override TypeClass TypeClass => TypeClass.TensorTensorTensor;

        public override string ToString()
        {
            return "(" + Children.First() + " % " + Children.Last() + ")";
        }

        public ModNode(IEnumerable<ExpressionNode> children) : base(children)
        {

        }
    }
}
