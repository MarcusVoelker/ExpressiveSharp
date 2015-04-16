using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Parser
{
    internal class AST
    {
        public override String ToString()
        {
            return Root.ToString();
        }

        public AST(ASTNode root)
        {
            Root = root;
        }

        public ASTNode Root { get; private set; }
    }

    internal class ASTNode
    {
        public new String ToString()
        {
            if (Children.Count == 0)
                return Token.ToString();
            return Token.ToString() + "(" + Children.Select(c => c.ToString()).Aggregate((l, r) => l + "," + r) + ")";
        }

        public ASTNode(Token token)
        {
            Token = token;
            Children = new List<ASTNode>();
        }

        public Token Token { get; private set; }

        public List<ASTNode> Children { get; private set; } 

    }


}
