using System.Globalization;

namespace ExpressiveSharp.Expression.Nodes
{
    internal class ConstantNode : LeafNode
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

        public override void Preprocess()
        {
            throw new System.NotImplementedException();
        }
    }
}
