﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression.Nodes
{
    internal abstract class BuiltinNode : FunctionNode
    {
        protected BuiltinNode(IEnumerable<ExpressionNode> children) : base(children)
        {
        }

        protected override ExpressionNode InternalPreprocess(Dictionary<string, TensorType> variableTypes)
        {
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
                    break;
                case TypeClass.Complex:
                    throw new InvalidOperationException("Complex functions need to override InternalPreprocess!");
                case TypeClass.Alias:
                    throw new InvalidOperationException("Alias functions need to derive AliasNode!");
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
