namespace ExpressiveSharp.Expression.Nodes
{
    internal class VariableNode : LeafNode
    {
        public VariableNode(string variable)
        {
            Variable = variable;
        }

        public string Variable { get; }
        public override string ToString()
        {
            return Variable;
        }
    }
}
