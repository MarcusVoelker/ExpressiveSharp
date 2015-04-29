using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ExpressiveSharp.Expression.Nodes
{
    internal abstract class BuiltinNode : FunctionNode
    {
        protected BuiltinNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }

        protected override ExpressionNode InternalPreprocess(Dictionary<string, TensorType> variableTypes)
        {
            ExpressionNode c0;
            ExpressionNode c1;
            switch (TypeClass)
            {
                case TypeClass.Tensors:
                    break;
                case TypeClass.TensorScalar:
                    break;
                case TypeClass.TensorTensor:
                    break;
                case TypeClass.ScalarTensorTensor:
                    break;
                case TypeClass.TensorScalarTensor:
                    break;
                case TypeClass.TensorTensorScalar:
                    break;
                case TypeClass.TensorTensorTensor:
                    c0 = children[0];
                    c1 = children[1];
                    Promote(ref c0, ref c1);
                    children[0] = c0;
                    children[1] = c1;
                    OutputType = Children[0].OutputType;
                    break;
                case TypeClass.Complex:
                    throw new InvalidOperationException("Complex functions need to override InternalPreprocess!");
                case TypeClass.Alias:
                    throw new InvalidOperationException("Alias functions need to derive AliasNode!");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return this;
        }

        public override bool IsConstant()
        {
            return Children.All(c => c.IsConstant());
        }

        public override Tensor GetConstant()
        {
            if (!IsConstant())
                throw new InvalidOperationException("Non-constant node!");

            return Evaluate(Children.Select(c => c.GetConstant()));
        }

        public override ExpressionNode FoldConstants()
        {
            if (IsConstant())
                return new ConstantNode(GetConstant());
            for (var i = 0; i < children.Count; ++i)
            {
                children[i] = children[i].FoldConstants();
            }
            return this;
        }

        protected abstract Tensor Evaluate(IEnumerable<Tensor> childrenTensors);
    }
}
