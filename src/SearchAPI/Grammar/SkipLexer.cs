using System;
using Sprache;

namespace SearchAPI.Grammar
{
    internal static class SkipLexer
    {
        internal static readonly Parser<object> Skip =
            from skip in Parse.IgnoreCase("_skip").Text()
            from delimeter in Parse.Char('=')
            from value in Parse.Number
            select new Skip(long.Parse(value));
    }

    public class Skip
    {
        public long Value { get; }

        public Skip(long value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Value = value;
        }
    }
}