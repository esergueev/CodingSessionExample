using Sprache;

namespace SearchAPI.Grammar
{
    internal static class OperatorsLexer
    {
        private static readonly Parser<Operator> Equal = ToOperator("eq", Operator.Equals);
        private static readonly Parser<Operator> NotEqual = ToOperator("ne", Operator.NotEqual);
        private static readonly Parser<Operator> GreaterThan = ToOperator("gt", Operator.GreaterThan);
        private static readonly Parser<Operator> GreaterOrEqual = ToOperator("ge", Operator.GreaterOrEqual);
        private static readonly Parser<Operator> LessThan = ToOperator("lt", Operator.LessThan);
        private static readonly Parser<Operator> LessOrEqual = ToOperator("le", Operator.LessOrEqual);

        internal static readonly Parser<Operator> DateTimeOperator = Equal
            .Or(NotEqual)
            .Or(GreaterThan)
            .Or(GreaterOrEqual)
            .Or(LessThan)
            .Or(LessOrEqual);

        private static Parser<Operator> ToOperator(string op, Operator opType)
        {
            return Parse.IgnoreCase(op).Token().Return(opType);
        }
    }
}