﻿using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class SubNode : BuiltinNode
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

        protected override Tensor Evaluate(IEnumerable<Tensor> childrenTensors)
        {
            var enumerable = childrenTensors as IList<Tensor> ?? childrenTensors.ToList();
            var t = new Tensor(enumerable.First().Type);
            return enumerable.Aggregate(t, (current, c) => current - c);
        }
    }
}
