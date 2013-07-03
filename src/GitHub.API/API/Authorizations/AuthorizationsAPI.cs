using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
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

        public IRestResponse<Authorization> CreateAuthorization(AuthorizationCreateOptions options)
        {
            var request = new RestRequest("/authorizations", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", options.Serialize(), ParameterType.RequestBody);

            return this._apiClient.ExecuteRequest<Authorization>(request);
        }

        public IRestResponse DeleteAuthorization(long id)
        {
            var request = new RestRequest("/authorizations/{id}", Method.DELETE);
            request.AddUrlSegment("id", id.ToString());

            return this._apiClient.ExecuteRequest(request);
        }

        public IRestResponse<Authorization> UpdateAuthorization(long id, AuthorizationUpdateOptions options)
        {
            var request = new RestRequest("/authorizations/{id}", Method.PATCH);
            request.AddUrlSegment("id", id.ToString());
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", options.Serialize(), ParameterType.RequestBody);

            return this._apiClient.ExecuteRequest<Authorization>(request);
        }
    }
}
