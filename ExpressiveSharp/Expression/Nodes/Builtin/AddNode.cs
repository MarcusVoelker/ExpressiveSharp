using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class AddNode : FunctionNode
    {
        public override string FunctionName => "Add";

        public override string ToString()
        {
            if (Children.Count == 1)
                return "+" + Children.First().ToString();
            return "(" + Children.First().ToString() + " + " + Children.Last().ToString() + ")";
        }

        public AddNode(List<ExpressionNode> children) : base(children)
        {
        }
    }
}
