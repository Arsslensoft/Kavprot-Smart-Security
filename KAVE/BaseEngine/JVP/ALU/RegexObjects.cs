using System;
using System.Text.RegularExpressions;

namespace JVP.ALN
{
    /// <summary>
    /// This class is internal to the library, houses different regular expression objects
    /// </summary>
    internal class DefinedRegex
    {
        private const string c_strNumeric       = @"(?:[0-9]+)?(?:\.[0-9]+)?(?:E-?[0-9]+)?(?=\b)";
        private const string c_strHex           = @"0x([0-9a-fA-F]+)";
        private const string c_strBool          = @"true|false";
        private const string c_strFunction      = @"\$(?<Function>\w+)\(";
        private const string c_strVariable      = @"\@\((?<Variable>[\@\w\s]+)\)";
        private const string c_strString        = "\\\"\\\"|\\\"(?<String>.*?[^\\\\])\\\"";

        private const string c_strUnaryOp       = @"(?:\+|SUB|NOT|BWC)(?=\w|\()";
        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private const string c_strBinaryOp = @"LSH|RSH|ADD|SUB|MUL|DIV|MOD|AND|OR|BAND|BOR|XOR|LEQ|NEQ|CGE|CLE|AEQ|CLT|CGT";
        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private const string c_strDate          = @"\d{1,2}[-/]\d{1,2}[-/](?:\d{4}|\d{2})" + 
                                                  @"(?:\s+\d{1,2}\:\d{2}(?:\:\d{2})?\s*(?:AM|PM)?)?";
        private const string c_strTimeSpan = @"(?:(?<Days>\d+)\.|)(?<Hours>\d{1,2})\:(?<Minutes>\d{1,2})" +
                                                  @"(?:\:(?<Seconds>\d{1,3})(?:\.(?<Milliseconds>\d{1,3})|)|)";
        private const string c_strWhiteSpace = @"\s+";

        internal static Regex Numeric = new Regex(
            c_strNumeric,
            RegexOptions.Compiled
        );

        internal static Regex Hexadecimal = new Regex(
            c_strHex,
            RegexOptions.Compiled
        );

        internal static Regex Boolean = new Regex(
            c_strBool,
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        internal static Regex UnaryOp = new Regex(
            @"(?<=(?:" + c_strBinaryOp + @")\s*|\A)(?:" + c_strUnaryOp + @")",
            RegexOptions.Compiled
        );

        internal static Regex BinaryOp = new Regex(
            @"(?<!(?:" + c_strBinaryOp + @")\s*|^\A)(?:" + c_strBinaryOp + @")",
            RegexOptions.Compiled
        );

        internal static Regex Parenthesis = new Regex(
            @"\(",
            RegexOptions.Compiled
        );

        internal static Regex Function = new Regex(
            c_strFunction,
            RegexOptions.Compiled
        );

        internal static Regex Variable = new Regex(
            c_strVariable,
            RegexOptions.Compiled
        );

        internal static Regex DateTime = new Regex(
            @"@dt\((?<DateString>" + c_strDate + @")\)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        internal static Regex TimeSpan = new Regex(
            @"@ts\(" + c_strTimeSpan + @"\)",
            RegexOptions.Compiled
        );

        internal static Regex String = new Regex(
            c_strString,
            RegexOptions.Compiled
        );

        internal static Regex WhiteSpace = new Regex(
            c_strWhiteSpace,
            RegexOptions.Compiled
        );

        static DefinedRegex() { }

    }
}
