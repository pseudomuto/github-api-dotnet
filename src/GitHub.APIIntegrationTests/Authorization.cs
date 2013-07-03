using GitHub.API;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIIntegrationTests
{
    public class Authorization
    {
        public class GetAuthorization
        {
            public class WhenAvailable : BasicAuthIntegrationTest
            {
                private IRestResponse<API.Authorization> _subject;

                protected override void Setup()
                {
                    var authorizations = this.APIClient.Authorizations().GetAllAuthorizations();
                    if (authorizations.Data.Count > 0)
                    {
                        this._subject = this.APIClient.Authorizations().GetAuthorization(authorizations.Data[0].Id);
                    }
                }

                [Fact]
                public void CompletesRequestWithoutError()
                {
                    Assert.Equal(ResponseStatus.Completed, this._subject.ResponseStatus);
                    Assert.Equal(HttpStatusCode.OK, this._subject.StatusCode);
                }

                [Fact]
                public void RetrievesAuthDetails()
                {
                    if (this._subject != null)
                    {                        
                        Assert.NotNull(this._subject.Data);                        
                    }
                }
            }

            public class WhenNotFound : BasicAuthIntegrationTest
            {
                private IRestResponse<API.Authorization> _subject;

                protected override void Setup()
                {
                    this._subject = this.APIClient.Authorizations().GetAuthorization(1);
                }

                [Fact]
                public void CompletesTheRequestWithoutError()
                {
                    Assert.Equal(ResponseStatus.Completed, this._subject.ResponseStatus);
                }

                [Fact]
                public void ReturnsA404Response()
                {
                    Assert.Equal(HttpStatusCode.NotFound, this._subject.StatusCode);                    
                }

                [Fact]
                public void SetsDataToUninitializedInstance()
                {
                    Assert.NotNull(this._subject.Data);
                    Assert.Equal(0, this._subject.Data.Id);
                }
            }            
        }
                
        public class GetAllAuthorizations : BasicAuthIntegrationTest
        {
            private IRestResponse<List<API.Authorization>> _subject;

            protected override void Setup()
            {
                this._subject = this.APIClient.Authorizations().GetAllAuthorizations();
            }

            [Fact]
            public void CanRetrieveAuthorizations()
            {
                Assert.Equal(HttpStatusCode.OK, this._subject.StatusCode);
                Assert.NotNull(this._subject);
                Assert.NotEmpty(this._subject.Data);
            }
        }

        public class CreateAuthorization : BasicAuthIntegrationTest
        {
            private IRestResponse<API.Authorization> _subject;

            protected override void Setup()
            {
                this._subject = this.APIClient.Authorizations().CreateAuthorization(new AuthorizationCreateOptions
                {
                    Note = "Testing API"
                }); 
            }

            [Fact]
            public void ReturnsA200Response()
            {
                Assert.Equal(HttpStatusCode.Created, this._subject.StatusCode);
            }

            [Fact]
            public void ReturnsATokenObject()
            {
                Assert.NotNull(this._subject.Data);
            }

            [Fact]
            public void CreatesANewToken()
            {
                Assert.True(this._subject.Data.Id > 0);
            }
        }
    }
}
