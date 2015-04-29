namespace ExpressiveSharp.Expression.Nodes
{
    internal abstract class LeafNode : ExpressionNode
    {
        public override ExpressionNode FoldConstants()
        {
            return this;
        }
    }
}
