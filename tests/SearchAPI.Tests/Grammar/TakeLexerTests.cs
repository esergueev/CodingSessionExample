using System.Linq;
using SearchAPI.Grammar;
using Xunit;

namespace SearchAPI.Tests.Grammar
{
    public class TakeLexerTests
    {
        [Theory]
        [InlineData("_take=100", 100)]
        [InlineData("_take=1", 1)]
        public void ShouldParseSingleTakeStatement(string input, long value)
        {
            var sut = SearchParamsExpression.Process(input).OfType<Take>().First();

            Assert.Equal(value, sut.Value);
        }
    }
}