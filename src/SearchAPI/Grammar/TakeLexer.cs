using Sprache;

namespace SearchAPI.Grammar
{
    internal static class TakeLexer
    {
        internal static readonly Parser<object> Take =
            (from keyword in Parse.IgnoreCase("_take")
            from delimeter in Parse.Char('=')
            from value in Parse.Number
            select new Take(long.Parse(value)))
            .Named(nameof(Take));


    }
}