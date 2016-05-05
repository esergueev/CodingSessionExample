using System.Collections.Generic;
using SearchAPI.Grammar;
using Sprache;

namespace SearchAPI
{
    public static class SearchParamsExpression
    {
        public static IEnumerable<object> Process(string expression)
        {
            var special = SortLexer.Sort
                .Or(TakeLexer.Take)
                .Or(SkipLexer.Skip)
                .Or(LastUpdatedLexer.LastUpdated);

            return special.DelimitedBy(Parse.Char('&')).End().Parse(expression);
        }
    }
}