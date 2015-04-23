using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class AddNode : FunctionNode
    {
        public override string FunctionName => "add";

        public override TypeClass TypeClass
            => Children.Count == 1 ? TypeClass.TensorTensor : TypeClass.TensorTensorTensor;

        public override string ToString()
        {
            if (Children.Count == 1)
                return "+" + Children.First();
            return "(" + Children.First() + " + " + Children.Last() + ")";
        }

        public AddNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }
    }
}
