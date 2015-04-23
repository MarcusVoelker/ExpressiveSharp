﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressiveSharp.Expression
{
    public class TensorType : IEnumerable<int>
    {
        private readonly List<int> dimensions;

        public TensorType(List<int> dimensions)
        {
            this.dimensions = dimensions;
        }

        public void Add(int value)
        {
            dimensions.Add(value);
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
    }
}
