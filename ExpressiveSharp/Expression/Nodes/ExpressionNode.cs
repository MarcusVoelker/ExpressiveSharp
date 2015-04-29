using System;
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

            if (n1.OutputType.IsScalar())
            {
                n1 = new TensorConstructorNode(new ConstantNode(n2.OutputType.ToTensor()), n1)
                {
                    OutputType = n2.OutputType
                };
                return;
            }

            if (n2.OutputType.IsScalar())
            {
                n2 = new TensorConstructorNode(new ConstantNode(n1.OutputType.ToTensor()), n2)
                {
                    OutputType = n1.OutputType
                };
                return;
            }

            throw new InvalidOperationException("Nodes " + n1 + " and " + n2 + " cannot be promoted!");
        }
        protected static void Promote(ref ExpressionNode n1, ref ExpressionNode n2, ref ExpressionNode n3)
        {
            Promote(ref n1, ref n2);
            Promote(ref n2, ref n3);
            Promote(ref n3, ref n1);
        }

        public TensorType OutputType { get; protected set; }

        public abstract override string ToString();

        public abstract ExpressionNode Preprocess(Dictionary<string,TensorType> variableTypes);

        public abstract ExpressionNode FoldConstants();

        public abstract Tensor GetConstant();

        public abstract bool IsConstant();
    }
}
