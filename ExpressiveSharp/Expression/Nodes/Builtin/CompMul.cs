using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp;

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
            var t = new Tensor(enumerable.First().Type,1);
            return enumerable.Aggregate(t, (current, c) => current * c);
        }

        protected override IEnumerable<LLVMValueRef> InternalBuildLLVM(LLVMBuilderRef builder, IEnumerable<IEnumerable<LLVMValueRef>> children)
        {
            var cs = children.ToList();
            var res = cs[0];
            for (var i = 1; i < cs.Count; ++i)
            {
                res = res.Zip(cs[i], (l, r) => LLVM.BuildFMul(builder, l, r, "mul"));
            }
            return res;
        }
    }
}
