using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class MulNode : FunctionNode
    {
        public override string FunctionName => "Mul";

        public override string ToString()
        {
            return "(" + Children.First().ToString() + " * " + Children.Last().ToString() + ")";
        }

        public MulNode(List<ExpressionNode> children) : base(children)
        {
        }
    }
}
