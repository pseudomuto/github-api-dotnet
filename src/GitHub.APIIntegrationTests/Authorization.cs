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
                }

                [Fact]
                public void ReturnsA200Response()
                {
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
                
        public class GetAllAuthorizations : BasicAuthIntegrationTest, IDisposable
        {
            private IRestResponse<List<API.Authorization>> _subject;
            private long _dummyId;

            protected override void Setup()
            {
                // create a token
                var created = this.APIClient.Authorizations().CreateAuthorization(new AuthorizationCreateOptions
                {
                    Note = "DeletableAuthToken"
                });

                Assert.Equal(HttpStatusCode.Created, created.StatusCode);

                this._dummyId = created.Data.Id;
                this._subject = this.APIClient.Authorizations().GetAllAuthorizations();
            }

            [Fact]
            public void CanRetrieveAuthorizations()
            {
                Assert.Equal(HttpStatusCode.OK, this._subject.StatusCode);
                Assert.NotNull(this._subject);
                Assert.NotEmpty(this._subject.Data);
            }

            public void Dispose()
            {
                // get rid of the dummy id
                this.APIClient.Authorizations().DeleteAuthorization(this._dummyId);
            }
        }

        public class CreateAuthorization : BasicAuthIntegrationTest, IDisposable
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
            public void CompletesTheRequestWithoutError()
            {
                Assert.Equal(ResponseStatus.Completed, this._subject.ResponseStatus);
            }

            [Fact]
            public void ReturnsA201Response()
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

            public void Dispose()
            {
                // cleanup
                this.APIClient.Authorizations().DeleteAuthorization(this._subject.Data.Id);
            }
        }

        public class DeleteAuthorization
        {
            public class WhenAvailable : BasicAuthIntegrationTest
            {
                private IRestResponse _subject;

                protected override void Setup()
                {
                    // create a token
                    var created = this.APIClient.Authorizations().CreateAuthorization(new AuthorizationCreateOptions
                    {
                        Note = "DeletableAuthToken"
                    });

                    Assert.Equal(HttpStatusCode.Created, created.StatusCode);

                    // delete the token
                    this._subject = this.APIClient.Authorizations().DeleteAuthorization(created.Data.Id);
                }

                [Fact]
                public void CompletesTheRequestWithoutError()
                {
                    Assert.Equal(ResponseStatus.Completed, this._subject.ResponseStatus);
                }

                [Fact]
                public void ReturnsA204Response()
                {
                    Assert.Equal(HttpStatusCode.NoContent, this._subject.StatusCode);
                }
            }

            public class WhenNotAvailable : BasicAuthIntegrationTest
            {
                private IRestResponse _subject;

                protected override void Setup()
                {
                    // delete the token
                    this._subject = this.APIClient.Authorizations().DeleteAuthorization(0);
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
            }
        }
    }
}
