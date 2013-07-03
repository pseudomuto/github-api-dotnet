using GitHub.API;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests.APIs
{
    public class UsersAPI
    {
        public class Constructor
        {
            [Fact]
            public void RequiresTheAPIInstanceToNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new GitHub.API.UsersAPI(null);
                });
            }
        }

        public class GetUser : ResourceTest
        {
            public class WhenGivenAUserName : APITest
            {
                private static readonly string USERNAME = "pseudomuto";

                public WhenGivenAUserName()
                {
                    this._basicAuthClient.Users().GetUser(USERNAME);
                }

                [Fact]
                public void UsesGETMethod()
                {
                    Assert.Equal(Method.GET, this.Subject.Method);
                }

                [Fact]
                public void RequestsCorrectEndpoint()
                {
                    Assert.Equal("/users/{user}", this.Subject.Resource);
                }

                [Fact]
                public void SetsURLSegmentForUser()
                {
                    var name = this.Subject.Parameters.First(p => p.Name.Equals("user"));
                    Assert.Equal(USERNAME, name.Value);
                }
            }

            public class WhenCalledWithoutAUserName : APITest
            {
                private static readonly string USERNAME = "pseudomuto";

                public WhenCalledWithoutAUserName()
                {
                    this._basicAuthClient.Users().GetUser();
                }

                [Fact]
                public void UsesGETMethod()
                {
                    Assert.Equal(Method.GET, this.Subject.Method);
                }

                [Fact]
                public void RequestsCorrectEndpoint()
                {
                    Assert.Equal("/user", this.Subject.Resource);
                }
            }

            [Fact]
            public void ParsesResponse()
            {
                var response = new RestResponse<User>
                {
                    Content = this.GetResourceString("Users.item.json")
                };

                var json = new JsonDeserializer();
                var data = json.Deserialize<User>(response);

                Assert.Equal(1, data.Id);       
                Assert.Equal("https://github.com/images/error/octocat_happy.gif", data.AvatarURL.ToString());
                Assert.Equal("somehexcode", data.GravatarId);
                Assert.Equal("https://api.github.com/users/octocat", data.URL.ToString());
                Assert.Equal("monalisa octocat", data.Name);
                Assert.Equal("GitHub", data.Company);
                Assert.Equal("https://github.com/blog", data.Blog);
                Assert.Equal("San Francisco", data.Location);
                Assert.Equal("octocat@github.com", data.Email);
                Assert.False(data.Hireable);
                Assert.Equal("There once was...", data.Bio);
                Assert.Equal(2, data.PublicRepos);
                Assert.Equal(1, data.PublicGists);
                Assert.Equal(20, data.Followers);
                Assert.Equal(0, data.Following);
                Assert.Equal("https://github.com/octocat", data.HtmlURL.ToString());
                Assert.Equal("User", data.Type);
                Assert.Equal(100, data.TotalPrivateRepos);
                Assert.Equal(100, data.OwnedPrivateRepos);
                Assert.Equal(81, data.PrivateGists);
                Assert.Equal(10000, data.DiskUsage);
                Assert.Equal(8, data.Collaborators);

                Assert.NotNull(data.Plan);
                Assert.Equal("Medium", data.Plan.Name);
                Assert.Equal(400, data.Plan.Space);
                Assert.Equal(10, data.Plan.Collaborators);
                Assert.Equal(20, data.Plan.PrivateRepos);

                var expectedDate = DateTime.Parse("2008-01-14T04:33:35Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
                Assert.Equal(expectedDate, data.CreatedAt);
            }
        }
    }
}
