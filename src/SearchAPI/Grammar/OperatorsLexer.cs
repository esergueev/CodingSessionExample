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

        private static readonly Parser<Operator> Text = ToOperator("text", Operator.Text);

        private static readonly Parser<Operator> In = ToOperator("in", Operator.In);
        private static readonly Parser<Operator> NotIn = ToOperator("not-in", Operator.NotIn);
        private static readonly Parser<Operator> Above = ToOperator("above", Operator.Above);
        private static readonly Parser<Operator> Below = ToOperator("below", Operator.Below);
        private static readonly Parser<Operator> Not = ToOperator("not", Operator.Not);


        private static readonly Parser<Operator> Contains = ToOperator("contains", Operator.Contains);
        private static readonly Parser<Operator> Exact = ToOperator("exact", Operator.Exact);

        private static readonly Parser<Operator> Intersect = ToOperator("intersect", Operator.Intersect);


        internal static readonly Parser<Operator> DateTimeOperators = Equal
            .Or(NotEqual)
            .Or(GreaterThan)
            .Or(GreaterOrEqual)
            .Or(LessThan)
            .Or(LessOrEqual);

        internal static readonly Parser<Operator> NumberOperators = DateTimeOperators;

        internal static readonly Parser<Operator> TokenModifiers = NotIn
            .Or(Above)
            .Or(Below)
            .Or(Not)
            .Or(In)
            .Or(Text);

        internal static readonly Parser<Operator> StringModifiers = Text
            .Or(Contains)
            .Or(Exact);

        internal static readonly Parser<Operator> GeometryModifiers = Intersect;

        private static Parser<Operator> ToOperator(string op, Operator opType)
        {
            return Parse.IgnoreCase(op).Token().Return(opType);
        }
    }
}