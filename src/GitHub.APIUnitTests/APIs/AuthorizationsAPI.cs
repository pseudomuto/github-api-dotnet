using GitHub.API;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests.APIs
{
    public class AuthorizationsAPI
    {
        public abstract class BaseAuthorizationAPITest : APITest
        {
            private IRestRequest _subject;

            protected IRestRequest Subject
            {
                get
                {
                    this._subject = this._subject ?? this._basicAuthClient.ProcessedRequest;
                    return this._subject;
                }
            }

        }

        public class Constructor
        {
            [Fact]
            public void RequiresTheAPIInstanceToNotBeNull()
            {                    
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new GitHub.API.AuthorizationsAPI(null);
                });
            }

            [Fact]
            public void VerifiesThatBasicAuthIsBeingUsed()
            {
                Assert.DoesNotThrow(() => new API.APIClient("user", "pass").Authorizations());
                Assert.Throws<NotSupportedException>(() => { new API.APIClient().Authorizations(); });
                Assert.Throws<NotSupportedException>(() => { new API.APIClient("someAuthToken").Authorizations(); });
            }
        }

        public class GetAllAuthorizations : BaseAuthorizationAPITest
        {
            public GetAllAuthorizations()
            {
                this._basicAuthClient.Authorizations().GetAllAuthorizations();
            }

            [Fact]
            public void UsesGETMethod()
            {
                Assert.Equal(Method.GET, this.Subject.Method);
            }

            [Fact]
            public void RequestsCorrectEndpoint()
            {
                Assert.Equal("/authorizations", this.Subject.Resource);
            }

            [Fact]
            public void ParsesResponse()
            {
                var response = new RestResponse<API.Authorization>
                {
                    Content = this.GetResourceString("Authorizations.list.json")
                };

                var json = new JsonDeserializer();
                var data = json.Deserialize<List<API.Authorization>>(response);
                Assert.Equal(1, data.Count);
            }
        }

        public class GetAuthorization : BaseAuthorizationAPITest
        {
            private static readonly long RESOURCE_ID = 1L;

            public GetAuthorization()
            {
                this._basicAuthClient.Authorizations().GetAuthorization(RESOURCE_ID);                
            }

            [Fact]
            public void UsesGETMethod()
            {
                Assert.Equal(Method.GET, this.Subject.Method);
            }

            [Fact]
            public void RequestsCorrectEndpoint()
            {
                Assert.Equal("/authorizations/{id}", this.Subject.Resource);
            }

            [Fact]
            public void SetsUrlSegmentForId()
            {
                var name = this.Subject.Parameters.First(p => p.Name.Equals("id"));
                Assert.Equal(RESOURCE_ID.ToString(), name.Value);
            }

            [Fact]
            public void ParsesResponse()
            {
                var response = new RestResponse<API.Authorization>
                {
                    Content = this.GetResourceString("Authorizations.item.json")
                };

                var json = new JsonDeserializer();
                var data = json.Deserialize<API.Authorization>(response);

                Assert.Equal(1, data.Id);
                Assert.Equal("https://api.github.com/authorizations/1", data.Url.ToString());
                Assert.Equal("abc123", data.Token);
                Assert.Equal("optional note", data.Note);
                Assert.Equal("http://optional/note/url", data.NoteUrl.ToString());

                var expectedDate = DateTime.Parse("2011-09-06T17:26:27Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
                Assert.Equal(expectedDate, data.CreatedAt);

                expectedDate = DateTime.Parse("2011-09-06T20:39:23Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
                Assert.Equal(expectedDate, data.UpdatedAt);

                Assert.NotEmpty(data.Scopes);
                Assert.Equal("public_repo", data.Scopes[0]);
            }            
        }

        public class CreateAuthorization : BaseAuthorizationAPITest
        {
            private AuthorizationCreateOptions _options = new AuthorizationCreateOptions
            {
                scopes = new string[] { "public_repo" },
                client_id = "SomeId",
                client_secret = "SomeSecret",
                note = "Some Note",
                note_url = new Uri("http://pseudomuto.com/")
            };

            public CreateAuthorization()
            {                
                this._basicAuthClient.Authorizations().CreateAuthorization(this._options);
            }

            [Fact]
            public void UsesPOSTMethod()
            {
                Assert.Equal(Method.POST, this.Subject.Method);
            }

            [Fact]
            public void RequestsCorrectEndpoint()
            {
                Assert.Equal("/authorizations", this.Subject.Resource);
            }

            [Fact]
            public void SetsRequestFormatToJSON()
            {                
                Assert.Equal(DataFormat.Json, this.Subject.RequestFormat);
            }

            [Fact]
            public void SetsRequestBody()
            {
                Assert.Equal(1, this.Subject.Parameters.Count(p => p.Type.Equals(ParameterType.RequestBody)));
            }
        }        
    }
}
