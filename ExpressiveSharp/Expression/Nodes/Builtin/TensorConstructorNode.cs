using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class TensorConstructorNode : BuiltinNode
    {
        public override string FunctionName => "tensor";

        public override TypeClass TypeClass => TypeClass.Complex;

        public TensorConstructorNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }

        public TensorConstructorNode(params ExpressionNode[] children) : base(children)
        {
        }

        protected override ExpressionNode InternalPreprocess(Dictionary<string, TensorType> variableTypes)
        {
            if (!children[0].IsConstant())
                throw new InvalidOperationException("Non-constant tensor size");

            OutputType = children[0].GetConstant().ToType();
            return this;
        }

        protected override Tensor Evaluate(IEnumerable<Tensor> childrenTensors)
        {
            var result = new Tensor(OutputType);
            var cTensors = childrenTensors.ToList();
            cTensors.RemoveAt(0);
            var cIndex = 0;
            var eIndex = 0;
            for (var i = 0; i < OutputType.ElementCount(); ++i)
            {
                result.Data[i] = cTensors[cIndex].Data[eIndex];
                eIndex++;
                if (eIndex < cTensors[cIndex].Type.ElementCount())
                    continue;
                eIndex = 0;
                cIndex = (cIndex + 1)%cTensors.Count;
            }
            return result;
        }

        protected override IEnumerable<LLVMValueRef> InternalBuildLLVM(LLVMBuilderRef builder, IEnumerable<IEnumerable<LLVMValueRef>> children)
        {
            var cs = children.Select(c => c.ToList()).ToList();
            cs.RemoveAt(0);
            var cIndex = 0;
            var eIndex = 0;
            for (var i = 0; i < OutputType.ElementCount(); ++i)
            {
                yield return cs[cIndex][eIndex];
                eIndex++;
                if (eIndex < this.children[cIndex+1].OutputType.ElementCount())
                    continue;
                eIndex = 0;
                cIndex = (cIndex + 1) % cs.Count;
            }
        }
    }
}
