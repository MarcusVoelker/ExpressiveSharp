using System;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp;

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

        public override ExpressionNode Preprocess(Dictionary<string, TensorType> variableTypes)
        {
            if (!variableTypes.ContainsKey(Variable))
                throw new InvalidOperationException("Untyped variable " + Variable);

            OutputType = variableTypes[Variable];
            return this;
        }

        public override ExpressionNode FoldConstants()
        {
            return this;
        }

        public override Tensor GetConstant()
        {
            throw new InvalidOperationException("Non-constant node!");
        }

        public override bool IsConstant()
        {
            return false;
        }

        public override IEnumerable<LLVMValueRef> BuildLLVM(LLVMBuilderRef builder, Dictionary<string, LLVMValueRef> vars)
        {
            for (var i = 0; i < OutputType.ElementCount(); ++i)
                yield return vars[Variable + "#" + i];
        }

        public override IEnumerable<Tuple<string, TensorType>> GetVariables()
        {
            yield return new Tuple<string,TensorType>(Variable,OutputType);
        }
    }
}
