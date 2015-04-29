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

        protected override Tensor Evaluate(IEnumerable<Tensor> childrenTensors)
        {
            var enumerable = childrenTensors as IList<Tensor> ?? childrenTensors.ToList();
            var t = new Tensor(enumerable.First().Type);
            return enumerable.Aggregate(t, (current, c) => current / c);
        }
    }
}
