using System.Collections.Generic;
using System.Linq;
using LLVMSharp;

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

        protected override IEnumerable<LLVMValueRef> InternalBuildLLVM(LLVMBuilderRef builder, IEnumerable<IEnumerable<LLVMValueRef>> children)
        {
            var cs = children.ToList();
            var res = cs[0];
            for (var i = 1; i < cs.Count; ++i)
            {
                res = res.Zip(cs[i], (l, r) => LLVM.BuildFAdd(builder, l, r, "add"));
            }
            return res;
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
