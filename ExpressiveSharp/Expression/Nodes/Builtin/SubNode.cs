using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class SubNode : FunctionNode
    {
        public override string FunctionName => "sub";

        public override TypeClass TypeClass
            => Children.Count == 1 ? TypeClass.TensorTensor : TypeClass.TensorTensorTensor;

        public override string ToString()
        {
            if (Children.Count == 1)
                return "-" + Children.First();
            return "(" + Children.First() + " - " + Children.Last() + ")";
        }

        public SubNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }
    }
}
