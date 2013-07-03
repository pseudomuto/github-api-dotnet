using GitHub.APIUnitTests.Fakes;
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

        protected void SetResourceName(string name)
        {
            this._noAuthClient.EmbeddedResourceName = this.CreateResourceName(name);
            this._tokenAuthClient.EmbeddedResourceName = this.CreateResourceName(name);
            this._basicAuthClient.EmbeddedResourceName = this.CreateResourceName(name);
        }
    }
}
