using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ExpressiveSharp.Expression.Nodes.Builtin;
using LLVMSharp;

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

        public abstract IEnumerable<LLVMValueRef> BuildLLVM(LLVMBuilderRef builder, Dictionary<string, LLVMValueRef> vars);

        public IEnumerable<LLVMValueRef> BuildLLVM()
        {
            var vars = GetVariables().ToList();
            vars.Sort();
            var module = LLVM.ModuleCreateWithName("JIT");
            var args = new LLVMTypeRef[1];
            var func = LLVM.AddFunction(module, "Test", LLVM.FunctionType(LLVM.DoubleType(), out args[0], (uint) vars.Count(), new LLVMBool(0)));

            var varDict = new Dictionary<string, LLVMValueRef>();
            foreach (var v in vars)
            {
                for (var i = 0; i < v.Item2.ElementCount(); ++i)
                    varDict[v.Item1 + "#" + i] = LLVM.GetParam(func, (uint)i);
            }

            return BuildLLVM(LLVM.CreateBuilder(),varDict);
        }

        public abstract IEnumerable<Tuple<string, TensorType>> GetVariables();
    }
}
