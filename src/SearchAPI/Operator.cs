namespace SearchAPI
{
    public enum Operator
    {
        //DateTime, Number
        Equals,
        NotEqual,
        GreaterThan,
        LessThan,
        LessOrEqual,
        GreaterOrEqual,

        //Token
        In,
        NotIn,
        Not,
        Above,
        Below,

        //String

        Contains,
        Exact,

        //Token, String
        Text,

        //Geometry
        Intersect
    }
}