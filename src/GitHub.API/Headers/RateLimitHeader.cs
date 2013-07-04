using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class RateLimitHeader
    {
        public int Limit { get; private set; }

        public int Remaining { get; private set; }

        public DateTime Reset { get; private set; }

        public RateLimitHeader(IRestResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");

            this.Limit = int.Parse(response.Headers.First(h => h.Name.Equals("X-RateLimit-Limit")).Value.ToString());
            this.Remaining = int.Parse(response.Headers.First(h => h.Name.Equals("X-RateLimit-Remaining")).Value.ToString());

            var epoch = long.Parse(response.Headers.First(h => h.Name.Equals("X-RateLimit-Reset")).Value.ToString());
            this.Reset = this.DateTimeFromEpoch(epoch);
        }

        private DateTime DateTimeFromEpoch(long seconds)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);            
            return start.ToLocalTime();
        }
    }
}
