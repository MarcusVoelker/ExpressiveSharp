using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression
{
    public class Tensor
    {
        public Tensor(TensorType type, IReadOnlyList<float> data)
        {
            Type = type;
            Data = new float[type.ElementCount()];
            for (var i = 0; i < Type.ElementCount(); ++i)
                Data[i] = i >= data.Count ? 0 : data[i];
        }
        public Tensor(TensorType type, params float[] data)
        {
            Type = type;
            Data = new float[type.ElementCount()];
            for (var i = 0; i < Type.ElementCount(); ++i)
                Data[i] = i >= data.Length ? 0 : data[i];
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
    }
}
