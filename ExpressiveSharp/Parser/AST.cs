using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Parser
{
    internal class AST
    {
        public override string ToString()
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
        public override string ToString()
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

        public Token Token { get; }

        public List<ASTNode> Children { get; }

        public Dictionary<string, ASTNode> Match(ASTNode matcher)
        {
            var dict = new Dictionary<string, ASTNode>();
            if (!(matcher.Token is IdentifierToken))
                return null;

            var id = (IdentifierToken) matcher.Token;
            if (matcher.Children.Count == 0)
            {
                dict[id.Name] = this;
                return dict;
            }

            if (matcher.Children.Count != Children.Count)
                return null;

            if (!(Token is IdentifierToken) || ((IdentifierToken) Token).Name != id.Name)
                return null;

            for (var i = 0; i < Children.Count; ++i)
            {
                var cDict = Children[i].Match(matcher.Children[i]);
                if (cDict == null)
                    return null;

                foreach (var p in cDict)
                    dict.Add(p.Key,p.Value);
            }
            return dict;
        }

        public ASTNode Replace(Dictionary<string, ASTNode> replacements)
        {
            var token = Token as IdentifierToken;
            if ((token != null) && Children.Count == 0 && replacements.ContainsKey(token.Name))
                return replacements[token.Name];

            var newAST = new ASTNode(Token);
            foreach(var c in Children)
                newAST.Children.Add(c.Replace(replacements));
            return newAST;
        }
    }


}
