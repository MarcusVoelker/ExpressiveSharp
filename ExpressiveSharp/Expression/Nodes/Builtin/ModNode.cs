using System;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class ModNode : BuiltinNode
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

        protected override Tensor Evaluate(IEnumerable<Tensor> childrenTensors)
        {
            var enumerable = childrenTensors as IList<Tensor> ?? childrenTensors.ToList();
            var t = new Tensor(enumerable.First().Type);
            return enumerable.Aggregate(t, (current, c) => current % c);
        }

        protected override IEnumerable<LLVMValueRef> InternalBuildLLVM(LLVMBuilderRef builder, IEnumerable<IEnumerable<LLVMValueRef>> children)
        {
            var cs = children.ToList();
            var res = cs[0];
            for (var i = 1; i < cs.Count; ++i)
            {
                res = res.Zip(cs[i], (l, r) =>LLVM.BuildFRem(builder, l, r, "mod"));
            }
            return res;
        }
    }
}
