using System.Linq;
using Xunit;

namespace SearchAPI.Tests
{
    public class ExpressionParserBuilderTests
    {
        [Fact]
        public void ShouldParse()
        {
            var builder = new ExpressionParserBuilder();
            builder
                .AddCondition("str", DataType.String)
                .AddCondition("number", DataType.Number)
                .AddCondition("dateTime", DataType.DateTime)
                .AddCondition("token", DataType.Token)
                ;

            var expression = "_skip=16&str=string value&number=10&unknown=some str &dateTime=2014&token=code1&_take=10";
            var sut = builder.Build().Invoke(expression).ToArray();

            Assert.Equal(expression.Split('&').Length, sut.Count());

            Assert.Equal(4, sut.OfType<ConditionExpression>().Count());
            Assert.Equal(1, sut.OfType<UndefinedExpression>().Count());
            Assert.Equal(2, sut.OfType<PagingExpression>().Count());

        } 
    }
}