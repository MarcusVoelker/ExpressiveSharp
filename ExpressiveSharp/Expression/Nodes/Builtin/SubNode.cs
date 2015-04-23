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
                return "-" + Children.First().ToString();
            return "(" + Children.First().ToString() + " - " + Children.Last().ToString() + ")";
        }

        public SubNode(List<ExpressionNode> children) : base(children)
        {
        }
    }
}
