using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes
{
    internal abstract class AliasNode : FunctionNode
    {
        protected abstract ExpressionNode Replace();

        public override TypeClass TypeClass => TypeClass.Alias;

        protected AliasNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }
    }
}
