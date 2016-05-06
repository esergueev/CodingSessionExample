using Sprache;

namespace SearchAPI.Grammar
{
    internal static class ConstantsLexer
    {
        internal static Parser<char> KeyValueDelimeterParser = Parse.Char('=');
        internal static Parser<char> ValueDelimeterParser = Parse.Char(',');
        internal static Parser<char> ColonParser = Parse.Char(':');
    }
}