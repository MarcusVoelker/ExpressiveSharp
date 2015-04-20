using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes
{
    internal class ConstantNode : ExpressionNode
    {
        public ConstantNode(float constant)
        {
            Constant = constant;
        }

        public float Constant { get; }
        public override string ToString()
        {
            return Constant.ToString(CultureInfo.InvariantCulture);
        }
    }
}
