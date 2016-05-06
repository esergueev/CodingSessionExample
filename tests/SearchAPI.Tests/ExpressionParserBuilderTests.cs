using System;
using System.Linq;
using Xunit;

namespace SearchAPI.Tests
{
    public class ExpressionParserBuilderTests
    {
        [Fact]
        public void ShouldComplexExpressionParse()
        {
            var builder = new ExpressionParserBuilder();
            builder
                .AddCondition("str", DataType.String)
                .AddCondition("number", DataType.Number)
                .AddCondition("dateTime", DataType.DateTime)
                .AddCondition("token", DataType.Token)
                ;

            var expression = "_sort:asc=token&_skip=16&str=string value &number=10&unknown=some str &dateTime=2014&token=code1&_take=10&_sort:desc=number";
            var sut = builder.Build().Invoke(expression).ToArray();

            Assert.Equal(expression.Split('&').Length, sut.Count());

            Assert.Equal(4, sut.OfType<ConditionExpression>().Count());
            Assert.Equal(1, sut.OfType<UndefinedExpression>().Count());
            Assert.Equal(2, sut.OfType<PagingExpression>().Count());
            Assert.Equal(2, sut.OfType<OrderingExpression>().Count());
        }

        [Theory]
        [MemberData(nameof(ParseNumbers))]
        [MemberData(nameof(ParseDateTime))]
        [MemberData(nameof(ParseToken))]
        [MemberData(nameof(ParseString))]
        public void ShouldParseNumbers(string column, DataType dataType, string expression, Operator op, object value)
        {
            //arrange
            var builder = new ExpressionParserBuilder();
            builder.AddCondition(column, dataType);

            //act
            var sut = builder.Build().Invoke(expression);

            //assert
            Assert.NotNull(sut.OfType<ConditionExpression>()
                .Single(exp => exp.Operator == op
                               && exp.Column.Equals(column)
                               && exp.Values.Any(v => v.Equals(value))));
        }

        private static TheoryData<string, DataType, string, Operator, object> ParseNumbers()
        {
            var set = new TheoryData<string, DataType, string, Operator, object>();
            string column = "f1";
            //Numbers


            var numberType = DataType.Number;
            var numberComporators = new[]
            {
                Operator.Equals,
                Operator.NotEqual,
                Operator.GreaterThan,
                Operator.GreaterOrEqual,
                Operator.LessOrEqual,
                Operator.LessThan,
            };
            //integer
            var longs = new[]
            {
                1L,
                -1L,
                10L,
                100L,
                100000L
                - 999999L
            }.Select(l => (object) l);

            //decimal
            var decimals = new[]
            {
                0.1,
                .1,
                -100.01,
                3.14159
            }.Select(d => (object) d);
            foreach (var numberComporator in numberComporators)
            {
                foreach (var value in longs.Concat(decimals))
                {
                    var expression = string.Join("=", column, string.Join("", OperatorToString(numberComporator), value));
                    set.Add(column, numberType, expression, numberComporator, value);
                    set.Add(column, numberType, string.Join("=", column, value), Operator.Equals, value);
                }
            }


            return set;
        }

        private static TheoryData<string, DataType, string, Operator, object> ParseDateTime()
        {
            var set = new TheoryData<string, DataType, string, Operator, object>();
            string column = "f1";

            var values = new[]
            {
                "2016",
                "2016-01",
                "2016-01-01",
                "2016-12-02T11:00:00",
                "2016-12-02T05:00:00+05:00",
                "2016-12-01T19:00:00-03:00",
                "2016-12-02T00:00:00Z",
                "2016-12-02T00:00:00.555Z",
            };

            var comporators = new[]
            {
                Operator.Equals,
                Operator.NotEqual,
                Operator.GreaterThan,
                Operator.GreaterOrEqual,
                Operator.LessOrEqual,
                Operator.LessThan,
            };

            foreach (var comporator in comporators)
            {
                foreach (var value in values)
                {
                    var expression = string.Join("=", column, string.Join("", OperatorToString(comporator), value));
                    set.Add(column, DataType.DateTime, expression, comporator, value);
                    set.Add(column, DataType.DateTime, string.Join("=", column, value), Operator.Equals, value);
                }
            }


            return set;
        }

        private static TheoryData<string, DataType, string, Operator, object> ParseToken()
        {
            var set = new TheoryData<string, DataType, string, Operator, object>();
            string column = "f1";

            var values = new[]
            {
                "code",
                "c0de",
                "1",
                "te-val",
                "_test"
            };

            var comporators = new[]
            {
                Operator.In,
                Operator.NotIn,
                Operator.Not,
                Operator.Above,
                Operator.Text,
                Operator.Below,
            };

            foreach (var comporator in comporators)
            {
                foreach (var value in values)
                {
                    set.Add(column, DataType.Token,
                        string.Join("=", string.Join(":", column, OperatorToString(comporator)), value), comporator,
                        value);
                    set.Add(column, DataType.Token, string.Join("=", column, value), Operator.Equals, value);
                }
            }


            return set;
        }

        private static TheoryData<string, DataType, string, Operator, object> ParseString()
        {
            var set = new TheoryData<string, DataType, string, Operator, object>();
            string column = "f1";

            var values = new[]
            {
                "code",
                "c0de",
                "1",
                "te-val",
                "_test",
                "value with space"
            };

            var comporators = new[]
            {
                Operator.Contains,
                Operator.Exact,
                Operator.Text,
            };

            foreach (var comporator in comporators)
            {
                foreach (var value in values)
                {
                    var expression = string.Join("=", string.Join(":", column, OperatorToString(comporator)), value);
                    set.Add(column, DataType.String, expression, comporator, value);
                    set.Add(column, DataType.String, string.Join("=", column, value), Operator.Equals, value);
                }
            }


            return set;
        }

        [Theory]
        [MemberData(nameof(ParsePaging))]
        public void ShouldParsePaging(string expression, Operator op, object value)
        {
            //arrange
            var builder = new ExpressionParserBuilder();

            //act
            var sut = builder.Build().Invoke(expression);

            //assert
            Assert.NotNull(sut.OfType<PagingExpression>()
                .Single(exp => exp.Paging == op
                               && exp.Value == uint.Parse(value.ToString())));
        }

        private static TheoryData< string, Operator, object> ParsePaging()
        {
            var set = new TheoryData< string, Operator, object>();

            var operators = new[]
            {
                Operator.Take,
                Operator.Skip,
            };

            var values = new[]
            {
                1,
                10,
                100,
                1000,
            };
            foreach (var op in operators)
            {
                foreach (var value in values)
                {
                    set.Add(string.Concat("_", OperatorToString(op), "=", value.ToString()), op, value);
                }
            }

            return set;
        }

        private static string OperatorToString(Operator op)
        {
            switch (op)
            {
                case Operator.Equals:
                    return "eq";
                case Operator.NotEqual:
                    return "ne";
                case Operator.GreaterThan:
                    return "gt";
                case Operator.LessThan:
                    return "lt";
                case Operator.LessOrEqual:
                    return "le";
                case Operator.GreaterOrEqual:
                    return "ge";
                case Operator.In:
                    return "in";
                case Operator.NotIn:
                    return "not-in";
                case Operator.Not:
                    return "not";
                case Operator.Above:
                    return "above";
                case Operator.Below:
                    return "below";
                case Operator.Contains:
                    return "contains";
                case Operator.Exact:
                    return "exact";
                case Operator.Text:
                    return "text";
                case Operator.Intersect:
                    break;
                case Operator.Take:
                    return "take";
                case Operator.Skip:
                    return "skip";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }

            throw new NotImplementedException();
        }
    }
}