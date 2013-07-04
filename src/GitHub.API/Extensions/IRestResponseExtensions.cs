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
        public static LinkHeader Links(this IRestResponse response)
        {
            return new LinkHeader(response.Headers.FirstOrDefault(h => h.Name.Equals("links", StringComparison.OrdinalIgnoreCase)).Value.ToString());
        }
    }
}
