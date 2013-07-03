using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.APIUnitTests.Fakes
{
    public class FakeAPIClient : API.APIClient
    {
        public string EmbeddedResourceName { get; set; }

        public FakeAPIClient() : base() { }
        public FakeAPIClient(string token) : base(token) { }
        public FakeAPIClient(string userName, string password) : base(userName, password) { }

        public IRestRequest ProcessedRequest { get; private set; }

        protected internal override IRestResponse ExecuteRequest(IRestRequest request)
        {
            this.PrepareRequest(request);
            this.ProcessedRequest = request;

            // integration tests will test results
            return null;
        }

        protected internal override IRestResponse<TModel> ExecuteRequest<TModel>(IRestRequest request)
        {
            this.PrepareRequest(request);
            this.ProcessedRequest = request;
            
            // integration tests will test results
            return null;
        }  

        private HttpStatusCode GetSuccessfulStatusCode(IRestRequest request)
        {
            var code = HttpStatusCode.OK;

            switch(request.Method)
            {
                case Method.POST:
                    code = HttpStatusCode.Created;
                    break;
            }

            return code;
        }
        
        class ResourceHelper : ResourceTest
        {
            public string GetEmbeddedResource(string name)
            {
                return this.GetResourceString(name, true);
            }
        }
    }
}
