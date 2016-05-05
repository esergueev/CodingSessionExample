using System.Linq;
using SearchAPI.Grammar;
using Xunit;

namespace SearchAPI.Tests.Grammar
{
    public class SkipLexerTests
    {
        [Theory]
        [InlineData("_skip=100", 100)]
        [InlineData("_skip=1", 1)]
        public void ShouldParseSingleSkipStatement(string input, long value)
        {
            var sut = SearchParamsExpression.Process(input).OfType<Skip>().First();

            Assert.Equal(value, sut.Value);
        }
    }
}