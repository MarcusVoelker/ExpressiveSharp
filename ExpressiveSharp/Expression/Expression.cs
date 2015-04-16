using System;

namespace ExpressiveSharp.Expression
{
    public class Expression
    {
        private ExpressionNode rootNode;

        public Expression(ExpressionNode rootNode)
        {
            this.rootNode = rootNode;
        }
    }

    public abstract class ExpressionNode
    {
        public abstract new String ToString();


    }
}
