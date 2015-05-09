using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        public Expression.RawExpressionFunction BuildLLVM()
        {
            var vars = GetVariables().ToList();
            vars.Sort();
            var module = LLVM.ModuleCreateWithName("JIT");
            var args = new[] {LLVM.PointerType(LLVM.FloatType(),0), LLVM.PointerType(LLVM.FloatType(),0) };
            var func = LLVM.AddFunction(module, "Test", LLVM.FunctionType(LLVM.VoidType(), out args[0], 2, new LLVMBool(0)));

            var bb = LLVM.AppendBasicBlock(func, "BB");
            var builder = LLVM.CreateBuilder();
            LLVM.PositionBuilderAtEnd(builder, bb);

            var varDict = new Dictionary<string, LLVMValueRef>();
            var inParam = LLVM.GetParam(func, 0);
            var outParam = LLVM.GetParam(func, 1);
            foreach (var v in vars)
            {
                for (var i = 0; i < v.Item2.ElementCount(); ++i)
                {
                    var indices = new[] {LLVM.ConstInt(LLVM.Int32Type(),(ulong) i,new LLVMBool(0))};
                    varDict[v.Item1 + "#" + i] = LLVM.BuildLoad(builder,LLVM.BuildGEP(builder,inParam,out indices[0],1,"inptr"),"inval");
                }
            }
            var rets = BuildLLVM(builder,varDict);
            int ctr = 0;
            foreach (var ret in rets)
            {
                var indices = new[] { LLVM.ConstInt(LLVM.Int32Type(), (ulong)ctr, new LLVMBool(0)) };
                var outVal = LLVM.BuildGEP(builder, outParam, out indices[0], 1, "outptr");
                LLVM.BuildStore(builder, ret, outVal);
                ctr++;
            }
            LLVM.BuildRetVoid(builder);

            LLVMExecutionEngineRef engine;

            LLVM.LinkInMCJIT();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();

            var platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32NT) // On Windows, LLVM currently (3.6) does not support PE/COFF
            {
                LLVM.SetTarget(module, Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) + "-elf");
            }

            LLVMMCJITCompilerOptions options;
            var optionsSize = (4 * sizeof(int)) + IntPtr.Size; // LLVMMCJITCompilerOptions has 4 ints and a pointer

            IntPtr error;

            LLVM.InitializeMCJITCompilerOptions(out options, optionsSize);
            LLVM.CreateMCJITCompilerForModule(out engine, module, out options, optionsSize, out error);

            var passManager = LLVM.CreateFunctionPassManagerForModule(module);
            LLVM.AddTargetData(LLVM.GetExecutionEngineTargetData(engine), passManager);
            LLVM.AddBasicAliasAnalysisPass(passManager);
            LLVM.AddPromoteMemoryToRegisterPass(passManager);
            LLVM.AddInstructionCombiningPass(passManager);
            LLVM.AddReassociatePass(passManager);
            LLVM.AddGVNPass(passManager);
            LLVM.AddCFGSimplificationPass(passManager);
            LLVM.InitializeFunctionPassManager(passManager);
            LLVM.RunFunctionPassManager(passManager, func);

            return (Expression.RawExpressionFunction) Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, func), typeof(Expression.RawExpressionFunction));
        }

        public abstract IEnumerable<Tuple<string, TensorType>> GetVariables();
    }
}
