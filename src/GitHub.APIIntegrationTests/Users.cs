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
            public class WhenGivenNoArguments : BasicAuthIntegrationTest
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
                public void SuppliesTheFirstPageURL()
                {
                    Assert.NotNull(this._subject.GetLinkHeader().First());
                }

                [Fact]
                public void SuppliesTheNextPageURL()
                {
                    Assert.NotNull(this._subject.GetLinkHeader().Next());
                }
            }            

            public class WhenGivenTheNextLink : TokenAuthIntegrationTest
            {
                private IRestResponse<List<User>> _subject;

                protected override void Setup()
                {
                    var response = this.APIClient.Users().GetAllUsers();
                    this._subject = this.APIClient.Users().GetAllUsers(response.GetLinkHeader().Next());
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
                public void SuppliesTheFirstPageURL()
                {
                    Assert.NotNull(this._subject.GetLinkHeader().First());
                }

                [Fact]
                public void SuppliesTheNextPageURL()
                {
                    Assert.NotNull(this._subject.GetLinkHeader().Next());
                }
            }
        }

        public class UpdateUser
        {
            public class WhenAuthenticated : TokenAuthIntegrationTest
            {
                private IRestResponse<User> _subject;
                
                public WhenAuthenticated()
                {
                    var response = this.APIClient.Users().GetUser();

                    var options = new UserUpdateOptions(true);
                    options.Name = response.Data.Name;
                    options.Email = response.Data.Email;
                    options.Blog = response.Data.Blog;
                    options.Company = response.Data.Company;
                    options.Location = response.Data.Location;
                    options.Hireable = response.Data.Hireable;
                    options.Bio = response.Data.Bio;

                    this._subject = this.APIClient.Users().UpdateUser(options);
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
                public void ReturnsTheUpdatedUser()
                {
                    Assert.NotNull(this._subject.Data);
                    Assert.Equal(ConfigurationManager.AppSettings["GITHUB_API_USERNAME"], this._subject.Data.Login);
                }
            }
        }
    }
}
