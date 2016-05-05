using Sprache;

namespace SearchAPI.Grammar
{
    internal static class SortLexer
    {
        internal static readonly Parser<object> Sort =
            from keyword in Parse.IgnoreCase("_sort").Text()
            from colon in Parse.Char(':').Optional()
            from direction in Parse.IgnoreCase("asc").Return(OrderingDirection.Ascending)
                .XOr(Parse.IgnoreCase("desc").Return(OrderingDirection.Descending))
                .Optional()
            from delimeter in Parse.Char('=')
            from column in Parse.LetterOrDigit.Many().Text()
            select new Ordering(column, direction.GetOrElse(OrderingDirection.Ascending));
    }
}