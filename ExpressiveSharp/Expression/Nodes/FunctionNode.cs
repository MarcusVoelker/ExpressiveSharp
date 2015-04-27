using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes
{
    internal abstract class FunctionNode : ExpressionNode
    {
        protected readonly List<ExpressionNode> children;

        protected FunctionNode(IEnumerable<ExpressionNode> children)
        {
            this.children = children.ToList();
        }

        public IReadOnlyList<ExpressionNode> Children => children;

        public abstract string FunctionName { get; }

        public abstract TypeClass TypeClass { get; }

        public override string ToString()
        {
            return FunctionName + "(" +
                   Children.Select(n => n.ToString()).Aggregate((l, r) => l + ", " + r) + ")";
        }

        protected abstract ExpressionNode InternalPreprocess(Dictionary<string, TensorType> variableTypes);

        public override ExpressionNode Preprocess(Dictionary<string, TensorType> variableTypes)
        {
            for (var i = 0; i < Children.Count; ++i)
                children[i] = children[i].Preprocess(variableTypes);

            return InternalPreprocess(variableTypes);
        }
    }
}
