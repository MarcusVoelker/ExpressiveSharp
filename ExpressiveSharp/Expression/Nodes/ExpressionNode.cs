using System.Dynamic;

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

        public TensorType OutputType { get; protected set; }

        public abstract override string ToString();

        public abstract void Preprocess();

    }
}
