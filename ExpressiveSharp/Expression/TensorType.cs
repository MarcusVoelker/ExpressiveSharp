using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressiveSharp.Expression
{
    public class TensorType : IEnumerable<int>, IEquatable<TensorType>
    {
        public bool Equals(TensorType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return dimensions.SequenceEqual(other.dimensions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TensorType) obj);
        }

        public override int GetHashCode()
        {
            return dimensions.GetHashCode();
        }

        public static bool operator ==(TensorType left, TensorType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TensorType left, TensorType right)
        {
            return !Equals(left, right);
        }

        private readonly List<int> dimensions;

        public TensorType(IEnumerable<int> dimensions)
        {
            this.dimensions = dimensions.ToList();
            while(this.dimensions.Count != 0 && this.dimensions.Last() == 0)
                this.dimensions.RemoveAt(this.dimensions.Count - 1);
        }

        public TensorType(params int[] dimensions)
        {
            this.dimensions = dimensions.ToList();
            while (this.dimensions.Count != 0 && this.dimensions.Last() == 0)
                this.dimensions.RemoveAt(this.dimensions.Count - 1);
        }

        public IReadOnlyList<int> Dimensions => dimensions;

        public IEnumerator<int> GetEnumerator()
        {
            return dimensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int ElementCount()
        {
            return dimensions.Aggregate(1, (current, i) => current*i);
        }

        public bool IsPromotableTo(TensorType other)
        {
            if (other.Dimensions.Count < Dimensions.Count)
                return false;

            return !Dimensions.Where((t, i) => other.Dimensions[i] != t).Any();
        }

        public bool IsScalar()
        {
            return Dimensions.Count == 0;
        }

        public Tensor ToTensor()
        {
            var type = new TensorType(Dimensions.Count);
            return new Tensor(type, dimensions.Select(i => (float) i).ToList());
        }
    }
}
