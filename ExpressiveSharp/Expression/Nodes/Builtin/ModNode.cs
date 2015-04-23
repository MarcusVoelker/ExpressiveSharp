using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class ModNode : FunctionNode
    {
        public override string FunctionName => "Mod";

        public override string ToString()
        {
            return "(" + Children.First() + " % " + Children.Last() + ")";
        }

        public ModNode(List<ExpressionNode> children) : base(children)
        {

        }
    }
}
