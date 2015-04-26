using System.Collections.Generic;

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
    }
}
