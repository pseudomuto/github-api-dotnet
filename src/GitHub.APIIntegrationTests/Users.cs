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

using GitHub.API;

namespace GitHub.APIIntegrationTests
{
    public class Users
    {
        public class GetUser
        {
            public class WhenSuppliedWithAUserName : BasicAuthIntegrationTest
            {
                private IRestResponse<User> _subject;

                protected override void Setup()
                {
                    this._subject = this.APIClient.Users().GetUser("pseudomuto");
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
                public void RetrievesUser()
                {
                    Assert.NotNull(this._subject.Data);
                    Assert.Equal("pseudomuto", this._subject.Data.Login);
                }

                [Fact]
                public void RetrievesUserPlan()
                {
                    Assert.NotNull(this._subject.Data.Plan);
                }
            }

            public class WithoutSpecifyingAUser : TokenAuthIntegrationTest
            {
                private IRestResponse<User> _subject;

                protected override void Setup()
                {
                    this._subject = this.APIClient.Users().GetUser();
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
                public void RetrievesUser()
                {
                    Assert.NotNull(this._subject.Data);
                    Assert.Equal(ConfigurationManager.AppSettings["GITHUB_API_USERNAME"], this._subject.Data.Login);
                }

                [Fact]
                public void RetrievesUserPlan()
                {
                    Assert.NotNull(this._subject.Data.Plan);
                }
            }
        }

        public class GetAllUsers
        {
            public class WhenGivenNoArguments : IntegrationTest
            {
                private IRestResponse<List<User>> _subject;

                protected override void Setup()
                {
                    this._subject = this.APIClient.Users().GetAllUsers();
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
                public void ReturnsSomeUsers()
                {
                    Assert.NotEmpty(this._subject.Data);
                }

                [Fact]
                public void SuppliesTheNextPageURL()
                {
                    Assert.NotNull(this._subject.Links().Next());
                }
            }            
        }
    }
}
