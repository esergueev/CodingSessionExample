using System;

namespace SearchAPI
{
    public class Take
    {
        public long Value { get; }

        public Take(long value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            
            Value = value;
        }
    }
}