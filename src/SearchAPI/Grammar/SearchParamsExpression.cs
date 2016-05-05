using System.Collections.Generic;
using Sprache;

namespace SearchAPI.Grammar
{
    public static class SearchParamsExpression
    {
        public static IEnumerable<object> Process(string expression)
        {
            var special = SortLexer.Sort
                .Or(TakeLexer.Take)
                .Or(SkipLexer.Skip);

            return special.DelimitedBy(Parse.Char('&')).End().Parse(expression);
        }
    }
}