using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.Infrastructure
{
    /// <summary>
    /// Range
    /// http://stackoverflow.com/questions/8948205/doing-a-range-lookup-in-c-sharp-how-to-implement/8949322#8949322
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class IntRange : IEnumerable<int>
    {
        public IntRange(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public int Max { get; set; }

        public int Min { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = this.Min; i <= this.Max; i++)
            {
                yield return i;
            }
        }
    }
}