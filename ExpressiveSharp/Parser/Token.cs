using System;
using System.Globalization;

namespace ExpressiveSharp.Parser
{
    internal abstract class Token
    {
        public new abstract string ToString();

        public virtual bool IsOperator(OperatorToken.OperatorType type)
        {
            return false;
        }
    }

    internal class ConstantToken : Token, IEquatable<ConstantToken>
    {
        public bool Equals(ConstantToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
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

        public float Value { get; }

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

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((IdentifierToken) obj);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }

        public IdentifierToken(string name)
        {
            Name = name;
        }

        public string Name { get; }

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

        public override string ToString()
        {
            return Type.ToString();
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
            Equal,
            Comma,
            Semicolon,
        }
        public OperatorToken(OperatorType type)
        {
            Type = type;
        }

        public OperatorType Type { get; }

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

        public override bool IsOperator(OperatorType type)
        {
            return Type == type;
        }
    }
}
