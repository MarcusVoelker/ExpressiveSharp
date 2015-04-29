using System;
using System.Collections.Generic;
using LLVMSharp;

namespace ExpressiveSharp.Expression.Nodes
{
    internal abstract class AliasNode : FunctionNode
    {
        protected abstract ExpressionNode Replace();

        public override TypeClass TypeClass => TypeClass.Alias;

        protected AliasNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }

        protected override ExpressionNode InternalPreprocess(Dictionary<string, TensorType> variableTypes)
        {
            return Replace().Preprocess(variableTypes);
        }

        public override ExpressionNode FoldConstants()
        {
            throw new InvalidOperationException("Leftover alias node!");
        }

        public override Tensor GetConstant()
        {
            throw new InvalidOperationException("Leftover alias node!");
        }

        public override bool IsConstant()
        {
            throw new InvalidOperationException("Leftover alias node!");
        }

        protected override IEnumerable<LLVMValueRef> InternalBuildLLVM(LLVMBuilderRef builder, IEnumerable<IEnumerable<LLVMValueRef>> children)
        {
            throw new InvalidOperationException("Leftover alias node!");
        }
    }
}
