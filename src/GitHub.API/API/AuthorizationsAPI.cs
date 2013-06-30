using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public partial class APIClient
    {
        public AuthorizationsAPI Authorizations()
        {
            if (this.AuthType != API.AuthType.Basic)
            {
                throw new NotSupportedException("Only basic authentication is allowed for the Authorizations API");
            }

            return new AuthorizationsAPI(this);
        }
    }

    public class AuthorizationsAPI
    {
        private APIClient _apiClient;

        internal AuthorizationsAPI(APIClient apiClientInstance)
        {
            if (apiClientInstance == null) throw new ArgumentNullException("apiClientInstance");

            this._apiClient = apiClientInstance;
        }

        public IRestResponse<List<Authorization>> GetAllAuthorizations()
        {            
            var request = new RestRequest("/authorizations", Method.GET);
            return this._apiClient.ExecuteRequest<List<Authorization>>(request);
        }

        public IRestResponse<Authorization> GetAuthorization(long id)
        {
            var request = new RestRequest("/authorizations/{id}");
            request.AddUrlSegment("id", id.ToString());

            return this._apiClient.ExecuteRequest<Authorization>(request);
        }
    }
}
