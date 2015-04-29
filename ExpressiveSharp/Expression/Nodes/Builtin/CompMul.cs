using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class CompMul : BuiltinNode
    {
        public override string FunctionName => "compMul";

        public override TypeClass TypeClass => TypeClass.TensorTensorTensor;

        public CompMul(IEnumerable<ExpressionNode> children) : base(children)
        {
        }

        protected override Tensor Evaluate(IEnumerable<Tensor> childrenTensors)
        {
            var enumerable = childrenTensors as IList<Tensor> ?? childrenTensors.ToList();
            var t = new Tensor(enumerable.First().Type);
            return enumerable.Aggregate(t, (current, c) => current * c);
        }
    }
}
