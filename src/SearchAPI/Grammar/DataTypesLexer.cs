using System.Linq;
using Sprache;

namespace SearchAPI.Grammar
{
    internal static class DataTypesLexer
    {
        private static readonly string DateTimePattern =
            @"[0-9]{4}(-(0[1-9]|1[0-2])(-(0[0-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9](\.[0-9]+)?(Z|(\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00))?)?)?)?";

        public static readonly Parser<object> DateTimeValueParser = Parse.Regex(DateTimePattern);

        private static readonly Parser<string> Integral =
            from sign in Parse.Char('-').Optional()
            from number in Parse.Number
            select sign.IsEmpty ? number : sign.Get() + number;

        private static readonly Parser<object> Long = Integral.Select(l => (object) long.Parse(l));

        private static readonly Parser<object> Double = 
            from integral in Integral
            from dot in Parse.Char('.')
            from val in Integral
            select (object) double.Parse(integral + "." + val);

        public static readonly Parser<object> NumberValueParser = Double
            .Or(Long);


        public static readonly Parser<object> TokenValueParser =
             from token in Parse.LetterOrDigit.XOr(Parse.Char('-')).XOr(Parse.Char('_')).Many().Text()
             select token;

        public static readonly Parser<object> StringValueParser = Parse.AnyChar.Except(Parse.Char('&')).AtLeastOnce().Text().Token();

    }
}