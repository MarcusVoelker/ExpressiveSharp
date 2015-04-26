using System;
using System.Collections.Generic;

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
    }
}
