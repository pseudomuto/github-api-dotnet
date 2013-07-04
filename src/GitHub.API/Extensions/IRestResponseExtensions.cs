using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public static class IRestResponseExtensions
    {
        public static LinkHeader GetLinkHeader<TModel>(this IRestResponse<List<TModel>> response) where TModel : new()
        {
            return new LinkHeader(response.Headers.FirstOrDefault(h => h.Name.Equals("link", StringComparison.OrdinalIgnoreCase)).Value.ToString());
        }

        public static RateLimitHeader GetRateLimit(this IRestResponse response)
        {
            return new RateLimitHeader(response);
        }
    }
}
