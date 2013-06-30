using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected internal override IRestResponse<TModel> ExecuteRequest<TModel>(IRestRequest request)
        {
            this.PrepareRequest(request);

            var content = new ResourceHelper().GetEmbeddedResource(this.EmbeddedResourceName);
            var response = new RestResponse<TModel>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.ResponseStatus = ResponseStatus.Completed;
            response.Content = content;

            var json = new JsonDeserializer();            
            response.Data = json.Deserialize<TModel>(response);

            return response;
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
