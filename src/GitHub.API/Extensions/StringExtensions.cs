using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHub.API
{
    public static class StringExtensions
    {
        public static string Parameterize(this string str, string separator = "_")
        {
            str = Regex.Replace(str, @"[^a-zA-Z0-9\-_]", separator);

            if (!string.IsNullOrEmpty(separator))
            {
                str = Regex.Replace(str, separator + "{2,}", separator);
                str = Regex.Replace(str, string.Format("^{0}|{0}$", separator), "");
            }

            return str.ToLower();
        }

        public static string Underscore(this string str)
        {            
            str = str.TrimStart('_').TrimEnd('_');

            str = Regex.Replace(str, @"[^a-zA-Z0-9]", "_");
            str = Regex.Replace(str, @"([A-Z\d]+)([A-Z][a-z])", "$1_$2");
            str = Regex.Replace(str, @"([a-z\d]+)([A-Z])", "$1_$2");

            return Regex.Replace(str.ToLower(), "_{2,}", "_");
        }
    }
}
