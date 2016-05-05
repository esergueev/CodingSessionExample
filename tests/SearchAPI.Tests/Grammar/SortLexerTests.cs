using System.Linq;
using SearchAPI.Grammar;
using Xunit;

namespace SearchAPI.Tests.Grammar
{
    public class SortLexerTests
    {
        [Theory]
        [InlineData("_sort=field", "field", OrderingDirection.Ascending)]
        [InlineData("_sort:desc=field", "field", OrderingDirection.Descending)]
        [InlineData("_sort:asc=field", "field", OrderingDirection.Ascending)]
        public void ShouldParseSingleSortStatement(string input, string column, OrderingDirection direction)
        {
            var sut = SearchParamsExpression.Process(input).OfType<Ordering>().First();

            Assert.Equal(column, sut.Column);
            Assert.Equal(direction, sut.Direction);
        }
    }
}