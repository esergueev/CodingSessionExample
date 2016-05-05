using System.Collections.Generic;
using Sprache;

namespace SearchAPI.Grammar
{
    internal static class LastUpdatedLexer
    {
        internal static readonly Parser<IEnumerable<Condition>> LastUpdated =
            from special in Parse.IgnoreCase("_lastUpdated")
            from delimeter in Parse.Char('=')
            from conditions in DataTypesLexer.DateTimeCondition.DelimitedBy(Parse.Char(','))
            select conditions;
    }
}