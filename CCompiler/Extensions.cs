using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCompiler
{
    public static class Extensions
    {
        public static bool IsHex(this char c)
        {
            return (c >= '0' && c <= '9') ||
                     (c >= 'a' && c <= 'f') ||
                     (c >= 'A' && c <= 'F');
        }

        public static bool IsOct(this char c)
        {
            return Regex.IsMatch(c.ToString(), "^[0-7]+$");
        }

        public static bool IsOctal(this string text)
        {
            return Regex.IsMatch(text, "^[0-7]+$");
        }
    }
}
