using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace vincontrol.Silverlight.NewLayout.Helpers
{
    public class ColorConverter
    {
        const String RgbRegex = @"(?<RGB>(?<R>[0-9a-fA-F])(?<G>[0-9a-fA-F])(?<B>[0-9a-fA-F]))";
        const String ArgbRegex = @"(?<ARGB>(?<A>[0-9a-fA-F])(?<R>[0-9a-fA-F])(?<G>[0-9a-fA-F])(?<B>[0-9a-fA-F]))";
        const String RrggbbRegex = @"(?<RGB>(?<R>[0-9a-fA-F]{2})(?<G>[0-9a-fA-F]{2})(?<B>[0-9a-fA-F]{2}))";
        const String AarrggbbRegex = @"(?<ARGB>(?<A>[0-9a-fA-F]{2})(?<R>[0-9a-fA-F]{2})(?<G>[0-9a-fA-F]{2})(?<B>[0-9a-fA-F]{2}))";

        public const Byte DefaultColorChannalValue = 255;

        public const String Color = @"#(" + RgbRegex + "|" + ArgbRegex + "|" + RrggbbRegex + "|" + AarrggbbRegex + ")";

        const String FullColorRegex = "^" + Color + "$";

        private static readonly Regex ColorRegexObject = new Regex(FullColorRegex);

        public static Color Convert(string input)
        {
            if (input != null && input.Trim().Length > 0)
            {
                Match match = ColorRegexObject.Match(input);
                if (match.Success)
                {
                    byte a = DefaultColorChannalValue;
                    if (match.Groups["ARGB"].Success)
                    {
                        a = GetByteFromHex(match.Groups["A"].Value);
                    }
                    byte r = GetByteFromHex(match.Groups["R"].Value);
                    byte g = GetByteFromHex(match.Groups["G"].Value);
                    byte b = GetByteFromHex(match.Groups["B"].Value);

                    return System.Windows.Media.Color.FromArgb(a, r, g, b);
                }
            }

            return Colors.Black;
        }

        private static Byte GetByteFromHex(String hex)
        {
            return Byte.Parse(hex, NumberStyles.HexNumber);
        }
    }
}