using System;
using System.Linq;
using Sprache;

namespace SearchAPI.Grammar
{
    public class NumberParserBuilder
    {
        private readonly string[] _columns;

        public NumberParserBuilder(params string[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columns.Any() == false)
            {
                throw new ArgumentOutOfRangeException(nameof(columns));
            }

            _columns = columns;
        }

        public Parser<FilertClause> Build()
        {
            return _columns
                .Select(c =>
                    from column in Parse.IgnoreCase(c).Text().Token()
                    from delimeter in Parse.Char('=')
                    from conditions in DataTypesLexer.NumberCondition.DelimitedBy(Parse.Char(','))
                    select new FilertClause(column, conditions.ToArray()))
                .Aggregate((a, b) => a.Or(b));
        }
    }

    public class FilertClause
    {
        public string Column { get; }
        public Condition[] Conditions { get; }

        public FilertClause(string column, Condition[] conditions)
        {
            if (string.IsNullOrEmpty(column))
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (conditions == null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            if (conditions.Any() == false)
            {
                throw new ArgumentOutOfRangeException(nameof(conditions));
            }

            Column = column;
            Conditions = conditions;
        }
    }
}