using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public enum AuthType
    {
        None = 0,
        Basic = 1,
        Token = 2
    }

    public partial class APIClient
    {
        private static readonly string API_HOST = "https://api.github.com";

        public AuthType AuthType { get; private set; }

        public string AuthValue { get; private set; }

        public APIClient()
        {
            this.AuthType = AuthType.None;
        }

        public APIClient(string authToken)
        {
            this.WithAuthToken(authToken);
        }

        public APIClient(string userName, string password)
        {
            this.WithCredentials(userName, password);
        }

        public APIClient WithAuthToken(string authToken)
        {
            if (string.IsNullOrEmpty(authToken)) throw new ArgumentNullException("authToken");

            this.AuthType = AuthType.Token;
            this.AuthValue = authToken;

            return this;
        }

        public APIClient WithCredentials(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            this.AuthType = AuthType.Basic;
            this.AuthValue = this.MakeAuthHeaderValue(userName, password);

            return this;
        }

        protected internal virtual IRestResponse<TModel> ExecuteRequest<TModel>(IRestRequest request) where TModel : new()
        {
            this.PrepareRequest(request);

            var client = new RestClient(API_HOST);
            return client.Execute<TModel>(request);
        }

        protected void PrepareRequest(IRestRequest request)
        {
            // add headers...
            this.SetContentType(request);
            this.SetAuthHeader(request);
        }

        private void SetContentType(IRestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
        }

        private void SetAuthHeader(IRestRequest request)
        {
            if (this.AuthType != API.AuthType.None)
            {
                request.AddHeader("Authorization", string.Format(
                        "{0} {1}",
                        this.AuthType == API.AuthType.Token ? "token" : "Basic",
                        this.AuthValue
                    ));
            }
        }

        private string MakeAuthHeaderValue(string userName, string password)
        {
            var raw = Encoding.UTF8.GetBytes(string.Concat(userName, ":", password));
            return Convert.ToBase64String(raw);
        }        
    }
}
