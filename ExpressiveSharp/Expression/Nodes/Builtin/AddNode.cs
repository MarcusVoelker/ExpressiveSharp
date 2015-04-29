using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class AddNode : BuiltinNode
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

        protected override Tensor Evaluate(IEnumerable<Tensor> childrenTensors)
        {
            var enumerable = childrenTensors as IList<Tensor> ?? childrenTensors.ToList();
            var t = new Tensor(enumerable.First().Type);
            return enumerable.Aggregate(t, (current, c) => current + c);
        }

        public AddNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }
    }
}
