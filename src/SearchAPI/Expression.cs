using System;
using System.Collections.Generic;
using System.Linq;
using SearchAPI.Grammar;
using Sprache;

namespace SearchAPI
{
    public abstract class Expression
    {
    }

    public class PagingExpression : Expression
    {
        public Paging Paging { get; }
        public uint Value { get; }

        public PagingExpression(Paging paging, uint value)
        {
            Paging = paging;
            Value = value;
        }
    }


    public class ConditionExpression : Expression
    {
        public string Column { get; }
        public Operator Operator { get; }
        public object[] Values { get; }

        public ConditionExpression(string column, Operator op, object[] values)
        {
            Column = column;
            Operator = op;

            Values = values;
        }
    }

    public class OrderingExpression : Expression
    {
        public string Column { get; }
        public OrderingDirection Direction { get; }

        public OrderingExpression(string column, OrderingDirection direction)
        {
            Column = column;
            Direction = direction;
        }
    }

    public class UndefinedExpression : Expression
    {
        public string Column { get; }

        public UndefinedExpression(string column)
        {
            Column = column;
        }
    }

    public enum Paging
    {
        Limit,
        Offset,
    }

    public enum DataType
    {
        Number,
        DateTime,
        Token,
        String,
    }

    public class ExpressionParserBuilder
    {
        private readonly IList<Parser<Expression>> _conditionParsers = new List<Parser<Expression>>();

        public ExpressionParserBuilder AddCondition(string column, DataType dataType)
        {
            return AddCondition(column, dataType, true);
        }

        public ExpressionParserBuilder AddCondition(string column, DataType dataType, bool sortable)
        {
            _conditionParsers.Add(MakeConditionParser(column, dataType, sortable));
            return this;
        }

        private Parser<Expression> MakeConditionParser(string column, DataType dataType, bool sortable)
        {
            Parser<string> columnParser = Parse.IgnoreCase(column).Text().Token();

            Parser<Expression> expression;
            switch (dataType)
            {
                case DataType.Number:
                    expression =
                        from col in columnParser
                        from del in ConstantsLexer.KeyValueDelimeterParser
                        from op in OperatorsLexer.NumberOperators.Optional()
                        from values in DataTypesLexer.NumberValueParser.DelimitedBy(ConstantsLexer.ValueDelimeterParser)
                        select new ConditionExpression(col, op.GetOrElse(Operator.Equals), values.ToArray());
                    break;

                case DataType.DateTime:
                    expression =
                        from col in columnParser
                        from del in ConstantsLexer.KeyValueDelimeterParser
                        from op in OperatorsLexer.DateTimeOperators.Optional()
                        from values in
                            DataTypesLexer.DateTimeValueParser.DelimitedBy(ConstantsLexer.ValueDelimeterParser)
                        select new ConditionExpression(col, op.GetOrElse(Operator.Equals), values.ToArray());
                    break;
                case DataType.Token:
                    expression =
                        from col in columnParser
                        from colon in ConstantsLexer.ColonParser.Optional()
                        from op in OperatorsLexer.TokenModifiers.Optional()
                        from del in ConstantsLexer.KeyValueDelimeterParser
                        from values in DataTypesLexer.TokenValueParser.DelimitedBy(ConstantsLexer.ValueDelimeterParser)
                        select new ConditionExpression(col, op.GetOrElse(Operator.Equals), values.ToArray());
                    break;
                case DataType.String:
                    expression =
                        from col in columnParser
                        from colon in ConstantsLexer.ColonParser.Optional()
                        from op in OperatorsLexer.StringModifiers.Optional()
                        from del in ConstantsLexer.KeyValueDelimeterParser
                        from value in DataTypesLexer.StringValueParser
                        select new ConditionExpression(col, op.GetOrElse(Operator.Equals), new[] {value});
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }

            return
                sortable
                    ? expression.Or(MakeSortParser(columnParser))
                    : expression;
        }

        private Parser<Expression> MakeSortParser(Parser<string> columnParser)
        {
            return
                from keyword in Parse.IgnoreCase("_sort").Text().Token()
                from colon in ConstantsLexer.ColonParser.Optional()
                from direction in Parse.IgnoreCase("asc").Return(OrderingDirection.Ascending)
                    .XOr(Parse.IgnoreCase("desc").Return(OrderingDirection.Descending))
                    .Optional()
                from del in ConstantsLexer.KeyValueDelimeterParser
                from column in columnParser
                select new OrderingExpression(column, direction.GetOrElse(OrderingDirection.Ascending));
        }

        private Parser<Expression> PagingParser()
        {
            return
                from paging in
                    Parse.IgnoreCase("_take").Return(Paging.Limit).Or(Parse.IgnoreCase("_skip").Return(Paging.Offset))
                from delimeter in ConstantsLexer.KeyValueDelimeterParser
                from value in Parse.Number
                select new PagingExpression(paging, uint.Parse(value));
        }

        private Parser<Expression> UndefinedParser()
        {
            return
                from column in DataTypesLexer.TokenValueParser
                from delimeter in ConstantsLexer.KeyValueDelimeterParser
                from value in DataTypesLexer.StringValueParser
                select new UndefinedExpression(column.ToString());
        }


        public Func<string, IEnumerable<Expression>> Build()
        {
            return
                (input) =>
                {
                    return _conditionParsers
                        .Aggregate((a, b) => a.Or(b))
                        .Or(PagingParser())
                        .Or(UndefinedParser())
                        .DelimitedBy(Parse.Char('&'))
                        .End()
                        .Parse(input);
                };
        }
    }
}