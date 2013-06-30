using GitHub.API;
using RestSharp;
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
        public class Constructor
        {
            public class WhenGivenAnAPIInstance
            {
                [Fact]
                public void RequiresTheInstanceToNotBeNull()
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
        }

        public class GetAllAuthorizations : APITest
        {
            public GetAllAuthorizations()
            {
                this.SetResourceName("list.json");                
            }

            [Fact]
            public void ReturnsAListOfAuthorizations()
            {
                var response = this._basicAuthClient.Authorizations().GetAllAuthorizations();
                Assert.NotEmpty(response.Data);
            }                       

            protected override string CreateResourceName(string name)
            {
                return base.CreateResourceName("Authorizations." + name);
            }
        }

        public class GetAuthorization : APITest
        {
            public GetAuthorization()
            {
                // comparison values can be found here
                this.SetResourceName("item.json");
            }

            [Fact]
            public void ReturnsAnAuthorizationInstance()
            {
                var response = this._basicAuthClient.Authorizations().GetAuthorization(1);
                Assert.NotNull(response.Data);
            }

            [Fact]
            public void SetsModelPropertiesCorrectly()
            {   
                var response = this._basicAuthClient.Authorizations().GetAuthorization(1);
                Assert.Equal(1, response.Data.Id);
                Assert.Equal("https://api.github.com/authorizations/1", response.Data.Url.ToString());
                Assert.Equal("abc123", response.Data.Token);
                Assert.Equal("optional note", response.Data.Note);
                Assert.Equal("http://optional/note/url", response.Data.NoteUrl.ToString());

                var expectedDate = DateTime.Parse("2011-09-06T17:26:27Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
                Assert.Equal(expectedDate, response.Data.CreatedAt);

                expectedDate = DateTime.Parse("2011-09-06T20:39:23Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
                Assert.Equal(expectedDate, response.Data.UpdatedAt);

                Assert.NotEmpty(response.Data.Scopes);
                Assert.Equal("public_repo", response.Data.Scopes[0]);
            }

            protected override string CreateResourceName(string name)
            {
                return base.CreateResourceName("Authorizations." + name);
            }
        }
    }
}
