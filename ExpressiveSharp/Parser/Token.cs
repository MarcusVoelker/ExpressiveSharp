using System;

namespace ExpressiveSharp.Parser
{
    internal abstract class Token
    {
    }

    internal class ConstantToken : Token, IEquatable<ConstantToken>
    {
        public bool Equals(ConstantToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ConstantToken) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public ConstantToken(float value)
        {
            Value = value;
        }

        public float Value { get; private set; }

        public static bool operator ==(ConstantToken lhs, Token rhs)
        {
            if (ReferenceEquals(lhs,null))
                return rhs == null;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return rhs is ConstantToken && lhs.Value == ((ConstantToken) rhs).Value;
        }

        public static bool operator !=(ConstantToken lhs, Token rhs)
        {
            return !(lhs == rhs);
        }
    }

    internal class IdentifierToken : Token, IEquatable<IdentifierToken>
    {
        public bool Equals(IdentifierToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((IdentifierToken) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public IdentifierToken(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static bool operator ==(IdentifierToken lhs, Token rhs)
        {
            if (ReferenceEquals(lhs,null))
                return rhs == null;
            return rhs is IdentifierToken && lhs.Name == ((IdentifierToken)rhs).Name;
        }

        public static bool operator !=(IdentifierToken lhs, Token rhs)
        {
            return !(lhs == rhs);
        }
    }

    internal class OperatorToken : Token, IEquatable<OperatorToken>
    {
        public bool Equals(OperatorToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((OperatorToken) obj);
        }

        public override int GetHashCode()
        {
            return (int) Type;
        }

        public enum OperatorType
        {
            Dot,
            LParen,
            RParen,
            Plus,
            Minus,
            Star,
            Slash,
            Percent,
        }
        public OperatorToken(OperatorType type)
        {
            Type = type;
        }

        public OperatorType Type { get; private set; }

        public static bool operator ==(OperatorToken lhs, Token rhs)
        {
            if (ReferenceEquals(lhs,null))
                return rhs == null;
            return rhs is OperatorToken && lhs.Type == ((OperatorToken)rhs).Type;
        }

        public static bool operator !=(OperatorToken lhs, Token rhs)
        {
            return !(lhs == rhs);
        }
    }
}
