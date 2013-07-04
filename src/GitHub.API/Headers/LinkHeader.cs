using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class LinkHeader
    {
        private static readonly Regex linkRegex = new Regex("^\\s*<([^>]+)>;\\s*rel=\"(\\w+)\"", RegexOptions.Compiled);
        
        private IDictionary<string, string> _values;

        public Uri Prev() { return this.ValueOrDefault("prev"); }

        public Uri Next() { return this.ValueOrDefault("next"); }

        public Uri First() { return this.ValueOrDefault("first"); }

        public Uri Last() { return this.ValueOrDefault("last"); }
            
        public LinkHeader(string headerValue)
        {
            if (string.IsNullOrEmpty(headerValue)) throw new ArgumentNullException("headerValue");

            this._values = new Dictionary<string, string>();
            foreach (var value in headerValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var match = linkRegex.Match(value);
                if (match != null)
                {
                    this._values.Add(match.Groups[2].Value, match.Groups[1].Value);
                }
            }
        }

        private Uri ValueOrDefault(string name)
        {
            if (!this._values.ContainsKey(name)) return null;

            return new Uri(this._values[name]);
        }
    }
}
