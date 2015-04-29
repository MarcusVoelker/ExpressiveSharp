using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LLVMSharp;

namespace ExpressiveSharp.Expression.Nodes
{
    internal class ConstantNode : LeafNode
    {
        public ConstantNode(Tensor constant)
        {
            Constant = constant;
        }

        public Tensor Constant { get; }
        public override string ToString()
        {
            return Constant.ToString();
        }

        public override ExpressionNode Preprocess(Dictionary<string, TensorType> variableTypes)
        {
            OutputType = Constant.Type;
            return this;
        }

        public override Tensor GetConstant()
        {
            return Constant;
        }

        public override bool IsConstant()
        {
            return true;
        }

        public override IEnumerable<LLVMValueRef> BuildLLVM(LLVMBuilderRef builder, Dictionary<string, LLVMValueRef> vars)
        {
            return Constant.Data.Select(t => LLVM.ConstReal(LLVM.FloatType(), t));
        }

        public override IEnumerable<Tuple<string, TensorType>> GetVariables()
        {
            yield break;
        }
    }
}
