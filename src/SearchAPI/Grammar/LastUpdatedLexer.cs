using System.Collections.Generic;
using Sprache;

namespace SearchAPI.Grammar
{
     internal static class LastUpdatedLexer
     {
         internal static readonly Parser<IEnumerable<Condition>> LastUpdated =
             from special in Parse.IgnoreCase("_lastUpdated")
             from delimeter in Parse.Char('=')
             from conditions in DateTimeCondition.DelimitedBy(Parse.Char(','))
             select conditions;

         private static readonly Parser<Condition> DateTimeCondition =
             from comporator in OperatorsLexer.DateTimeOperator.Optional()
             from validDateTime in Parse.Regex(@"[0-9]{4}(-(0[1-9]|1[0-2])(-(0[0-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9](\.[0-9]+)?(Z|(\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00)))?)?)?")
             select new Condition(comporator.GetOrElse(Operator.Equals), validDateTime);
     }
}