using System;
using System.Linq;
using SearchAPI.Grammar;
using Sprache;
using Xunit;

namespace SearchAPI.Tests
{
    public class NumberParserBuilderTests
    {
        [Theory]
        [InlineData("test", "", 1L, Operator.Equals)]
        [InlineData("test", "ne", 1L, Operator.NotEqual)]
        [InlineData("test", "ne", 3.14159265358979d, Operator.NotEqual)]
        [InlineData("test", "ne", 10.01d, Operator.NotEqual)]
        public void ShouldParseNumber(string column, string comporator, object value, Operator op)
        {
            //arrange
            var builder = new NumberParserBuilder(column);
            string expression = string.Concat(column, "=", comporator, value);

            //act
            var sut = builder.Build().Parse(expression);

            //assert
            Assert.Equal(column, sut.Column);
            Assert.Equal(op, sut.Conditions.First().Comporator);
            Assert.Equal(value, sut.Conditions.First().Value);
        }


    }
}