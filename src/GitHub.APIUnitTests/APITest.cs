using GitHub.APIUnitTests.Fakes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.APIUnitTests
{
    public abstract class APITest : ResourceTest
    {
        protected FakeAPIClient _noAuthClient = new FakeAPIClient();
        protected FakeAPIClient _tokenAuthClient = new FakeAPIClient("myAuthToken");
        protected FakeAPIClient _basicAuthClient = new FakeAPIClient("user", "pass");

        private IRestRequest _subject;

        protected IRestRequest Subject
        {
            get
            {
                this._subject = this._subject ?? this._basicAuthClient.ProcessedRequest;
                return this._subject;
            }
        }

        protected void SetResourceName(string name)
        {
            this._noAuthClient.EmbeddedResourceName = this.CreateResourceName(name);
            this._tokenAuthClient.EmbeddedResourceName = this.CreateResourceName(name);
            this._basicAuthClient.EmbeddedResourceName = this.CreateResourceName(name);
        }
    }
}
