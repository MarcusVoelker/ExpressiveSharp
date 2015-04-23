using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class SubNode : FunctionNode
    {
        public override string FunctionName => "Sub";

        public override string ToString()
        {
            if (Children.Count == 1)
                return "-" + Children.First();
            return "(" + Children.First() + " - " + Children.Last() + ")";
        }

        public SubNode(List<ExpressionNode> children) : base(children)
        {
        }
    }
}
