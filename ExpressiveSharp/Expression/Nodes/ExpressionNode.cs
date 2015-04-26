using System.Collections.Generic;
using System.Dynamic;
using ExpressiveSharp.Expression.Nodes.Builtin;

namespace ExpressiveSharp.Expression.Nodes
{
    internal enum TypeClass
    {
        Tensors,
        TensorScalar,
        TensorTensor,
        ScalarTensorTensor,
        TensorScalarTensor,
        TensorTensorScalar,
        TensorTensorTensor,
        Complex,
        Alias
    }

    internal abstract class ExpressionNode
    {
        protected static void Promote(ref ExpressionNode n1, ref ExpressionNode n2)
        {
            if (n1.OutputType == n2.OutputType)
                return;

            if (n1.OutputType.IsPromotableTo(n2.OutputType))
            {
                n2 = new TensorConstructorNode(new ConstantNode(n1.OutputType.ToTensor()), n2);
            }
        }

        public TensorType OutputType { get; protected set; }

        public abstract override string ToString();

        public abstract ExpressionNode Preprocess(Dictionary<string,TensorType> variableTypes);

    }
}
