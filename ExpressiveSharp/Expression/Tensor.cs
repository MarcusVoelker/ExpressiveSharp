using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace ExpressiveSharp.Expression
{
    public class Tensor
    {
        public Tensor(TensorType type, IReadOnlyList<float> data)
        {
            Type = type;
            Data = new float[type.ElementCount()];
            for (var i = 0; i < Type.ElementCount(); ++i)
                if (data.Count == 0)
                    Data[i] = 0;
                else
                    Data[i] = data[i % data.Count];
        }
        public Tensor(TensorType type, params float[] data)
        {
            Type = type;
            Data = new float[type.ElementCount()];
            for (var i = 0; i < Type.ElementCount(); ++i)
                if (data.Length == 0)
                    Data[i] = 0;
                else
                    Data[i] = data[i%data.Length];
        }

        public static Tensor Scalar(float v)
        {
            return new Tensor(new TensorType(),v);
        }

        public TensorType Type { get; }

        public float[] Data { get; }

        public override string ToString()
        {
            if (Data.Length == 1)
                return Data[0].ToString(CultureInfo.InvariantCulture);
            
            return "(" + Data.Select(i => i.ToString(CultureInfo.InvariantCulture)).Aggregate((l, r) => l + ", " + r) + ")" + ":" + Type;
        }

        public static implicit operator Tensor(float x)
        {
            return new Tensor(new TensorType(), x);
        }

        public static Tensor operator+(Tensor lhs, Tensor rhs)
        {
            if (lhs.Type != rhs.Type)
                throw new InvalidOperationException("Non-matching Tensor types " + lhs.Type + " and " + rhs.Type);

            var result = new Tensor(lhs.Type,lhs.Data);
            for (var i = 0; i < rhs.Type.ElementCount(); ++i)
            {
                result.Data[i] += rhs.Data[i];
            }
            return result;
        }

        public static Tensor operator -(Tensor lhs, Tensor rhs)
        {
            if (lhs.Type != rhs.Type)
                throw new InvalidOperationException("Non-matching Tensor types " + lhs.Type + " and " + rhs.Type);

            var result = new Tensor(lhs.Type, lhs.Data);
            for (var i = 0; i < rhs.Type.ElementCount(); ++i)
            {
                result.Data[i] -= rhs.Data[i];
            }
            return result;
        }

        public static Tensor operator *(Tensor lhs, Tensor rhs)
        {
            if (lhs.Type != rhs.Type)
                throw new InvalidOperationException("Non-matching Tensor types " + lhs.Type + " and " + rhs.Type);

            var result = new Tensor(lhs.Type, lhs.Data);
            for (var i = 0; i < rhs.Type.ElementCount(); ++i)
            {
                result.Data[i] *= rhs.Data[i];
            }
            return result;
        }

        public static Tensor operator /(Tensor lhs, Tensor rhs)
        {
            if (lhs.Type != rhs.Type)
                throw new InvalidOperationException("Non-matching Tensor types " + lhs.Type + " and " + rhs.Type);

            var result = new Tensor(lhs.Type, lhs.Data);
            for (var i = 0; i < rhs.Type.ElementCount(); ++i)
            {
                result.Data[i] /= rhs.Data[i];
            }
            return result;
        }

        public static Tensor operator %(Tensor lhs, Tensor rhs)
        {
            if (lhs.Type != rhs.Type)
                throw new InvalidOperationException("Non-matching Tensor types " + lhs.Type + " and " + rhs.Type);

            var result = new Tensor(lhs.Type, lhs.Data);
            for (var i = 0; i < rhs.Type.ElementCount(); ++i)
            {
                result.Data[i] %= rhs.Data[i];
            }
            return result;
        }

        public TensorType ToType()
        {
            return new TensorType(Data.Select(f => (int) f));
        }

        public float this[int i] => Data[i];
    }
}
