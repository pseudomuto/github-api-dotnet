using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class UsersAPI : BaseAPI
    {
        internal UsersAPI(APIClient apiClientInstance)
            : base(apiClientInstance)
        {
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

            return this.APIClient.ExecuteRequest<User>(request);
        }

        public IRestResponse<List<User>> GetAllUsers(Uri link = null)
        {
            IRestRequest request = new RestRequest("/users");

            if (link != null)
            {
                request = this.APIClient.MakeRequestFromUri(link);
            }

            return this.APIClient.ExecuteRequest<List<User>>(request);
        }

        public IRestResponse<User> UpdateUser(UserUpdateOptions options)
        {
            var request = new RestRequest("/user", Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", options.Serialize(), ParameterType.RequestBody);

            return this.APIClient.ExecuteRequest<User>(request);
        }

        public IRestResponse<List<UserEmail>> GetEmails()
        {
            var request = new RestRequest("/user/emails");
            return this.APIClient.ExecuteRequest<List<UserEmail>>(request);
        }

        public IRestResponse<List<string>> AddEmails(params string[] emails)
        {
            var request = new RestRequest("/user/emails", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(emails);

            return this.APIClient.ExecuteRequest<List<string>>(request);
        }

        public IRestResponse<List<string>> DeleteEmails(params string[] emails)
        {
            var request = new RestRequest("/user/emails", Method.DELETE);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(emails);

            return this.APIClient.ExecuteRequest<List<string>>(request);
        }
    }
}
