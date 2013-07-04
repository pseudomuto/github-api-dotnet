using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class UsersAPI
    {
        private APIClient _apiClient;

        internal UsersAPI(APIClient apiClientInstance)
        {
            if (apiClientInstance == null) throw new ArgumentNullException("apiClientInstance");

            this._apiClient = apiClientInstance;
        }

        public IRestResponse<User> GetUser(string userName = null)
        {
            IRestRequest request;

            if (string.IsNullOrEmpty(userName))
            {
                request = new RestRequest("/user");
            }
            else
            {
                request = new RestRequest("/users/{user}");
                request.AddUrlSegment("user", userName);
            }

            return this._apiClient.ExecuteRequest<User>(request);
        }

        public IRestResponse<List<User>> GetAllUsers(Uri link = null)
        {
            IRestRequest request = new RestRequest("/users");

            if (link != null)
            {
                request = this._apiClient.MakeRequestFromUri(link);
            }

            return this._apiClient.ExecuteRequest<List<User>>(request);
        }

        public IRestResponse<User> UpdateUser(UserUpdateOptions options)
        {
            var request = new RestRequest("/user", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", options.Serialize(), ParameterType.RequestBody);

            return this._apiClient.ExecuteRequest<User>(request);
        }

        public IRestResponse<List<UserEmail>> GetEmails()
        {
            var request = new RestRequest("/user/emails");
            return this._apiClient.ExecuteRequest<List<UserEmail>>(request);
        }
    }
}
