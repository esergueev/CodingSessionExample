using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SearchAPI.Tests.Grammar
{
    public class LastUpdatedLexerTests
    {
        [Theory]
        [InlineData("_lastUpdated=2014", "2014", Operator.Equals)]
        [InlineData("_lastUpdated=eq2014-01", "2014-01", Operator.Equals)]
        [InlineData("_lastUpdated=ne2014-01", "2014-01", Operator.NotEqual)]
        [InlineData("_lastUpdated=ge2014-01-15", "2014-01-15", Operator.GreaterOrEqual)]
        [InlineData("_lastUpdated=gt2014-01-15", "2014-01-15", Operator.GreaterThan)]
        [InlineData("_lastUpdated=lt2014-01-15", "2014-01-15", Operator.LessThan)]
        [InlineData("_lastUpdated=le2014-01-15", "2014-01-15", Operator.LessOrEqual)]
        public void ShouldParseSingleDateTimeStatement(string input, string dateTime, Operator comporator)
        {
            var sut = SearchParamsExpression.Process(input).OfType<IEnumerable<Condition>>().First().First();

            Assert.Equal(dateTime, sut.Value);
            Assert.Equal(comporator, sut.Comporator);

        }
    }
}