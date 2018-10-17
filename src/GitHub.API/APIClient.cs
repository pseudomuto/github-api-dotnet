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

    #region [Workaround until new NuGet package is supplied]

    public class GitHubOAuthProvider : OAuth2AuthorizationRequestHeaderAuthenticator
    {
        public GitHubOAuthProvider(string accessToken)            
            : base(accessToken)
        {
        }

        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            if (!request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
            {
                request.AddParameter("Authorization", "token "+ this.AccessToken, ParameterType.HttpHeader);
            }
        }
    }

    #endregion

    public partial class APIClient
    {
        private static readonly string API_HOST = "https://api.github.com";

        public AuthType AuthType { get; private set; }
        
        public IAuthenticator Authenticator { get; private set; }

        public APIClient()
        {
            this.AuthType = AuthType.None;
            this.Authenticator = null;
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
            this.Authenticator = new GitHubOAuthProvider(authToken);

            return this;
        }

        public APIClient WithCredentials(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            this.AuthType = AuthType.Basic;
            this.Authenticator = new HttpBasicAuthenticator(userName, password);
            
            return this;
        }

        #region [Individual APIs]

        public AuthorizationsAPI Authorizations()
        {
            if (this.AuthType != API.AuthType.Basic)
            {
                throw new NotSupportedException("Only basic authentication is allowed for the Authorizations API");
            }

            return new AuthorizationsAPI(this);
        }

        public UsersAPI Users()
        {
            return new UsersAPI(this);
        }

        #endregion

        protected internal virtual IRestResponse ExecuteRequest(IRestRequest request)
        {
            this.PrepareRequest(request);

            var client = new RestClient(API_HOST);
            if (this.Authenticator != null)
            {
                client.Authenticator = this.Authenticator;
            }

            return client.Execute(request);
        }

        protected internal virtual IRestResponse<TModel> ExecuteRequest<TModel>(IRestRequest request) where TModel : new()
        {
            this.PrepareRequest(request);

            var client = new RestClient(API_HOST);
            if (this.Authenticator != null)
            {
                client.Authenticator = this.Authenticator;
            }

            return client.Execute<TModel>(request);
        }

        protected internal IRestRequest MakeRequestFromUri(Uri link)
        {
            var request = new RestRequest(link.AbsolutePath);
            if (!string.IsNullOrEmpty(link.Query))
            {
                string[] parts;
                var parameters = link.Query.TrimStart('?').Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var param in parameters)
                {
                    parts = param.Split('=');
                    request.AddParameter(parts[0], parts[1]);
                }
            }

            return request;
        }

        protected void PrepareRequest(IRestRequest request)
        {
            // add headers...
            this.SetContentType(request);
            // Github v3 requires HTTP User-Agent header
            // https://developer.github.com/v3/#user-agent-required
            this.SetUserAgent(request);
        }

        private void SetContentType(IRestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
        }

        private void SetUserAgent(IRestClient request)
        {
            request.AddHeader("User-Agent", "github-api-dotnet (https://github.com/pseudomuto/github-api-dotnet)");
        }
    }    
}
