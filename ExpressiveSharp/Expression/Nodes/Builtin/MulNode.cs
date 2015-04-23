using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression.Nodes.Builtin
{
    internal class MulNode : AliasNode
    {
        public override string FunctionName => "mul";

        public override string ToString()
        {
            return "(" + Children.First() + " * " + Children.Last() + ")";
        }

        protected override ExpressionNode Replace()
        {
            return new CompMul(Children.ToList());
        }

        public MulNode(IEnumerable<ExpressionNode> children) : base(children)
        {

        }
    }
}
