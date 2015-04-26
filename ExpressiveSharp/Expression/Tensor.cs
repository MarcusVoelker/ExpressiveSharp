using System;
using System.Collections.Generic;
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
            Data = new List<float>(type.ElementCount());
            for (var i = 0; i < Type.ElementCount(); ++i)
                Data[i] = i >= data.Count ? 0 : data[i];
        }
        public Tensor(TensorType type, params float[] data)
        {
            Type = type;
            Data = new List<float>(type.ElementCount());
            for (var i = 0; i < Type.ElementCount(); ++i)
                Data[i] = i >= data.Length ? 0 : data[i];
        }

        public static Tensor Scalar(float v)
        {
            return new Tensor(new TensorType(),v);
        }

        public TensorType Type { get; }

        public List<float> Data { get; }

        public override string ToString()
        {
            return Data + ":" + Type.Dimensions;
        }

        public static implicit operator Tensor(float x)
        {
            return new Tensor(new TensorType(), x);
        }
    }
}
