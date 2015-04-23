using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class DivNode : FunctionNode
    {
        public override string FunctionName => "Div";

        public override string ToString()
        {
            return "(" + Children.First().ToString() + " / " + Children.Last().ToString() + ")";
        }

        public DivNode(List<ExpressionNode> children) : base(children)
        {

        }
    }
}
