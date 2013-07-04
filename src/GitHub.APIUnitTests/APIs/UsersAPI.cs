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
                Assert.Equal("https://github.com/blog", data.Blog.ToString());
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

        public class GetAllUsers 
        {
            public class WhenGivenNoArguments : APITest
            {
                public WhenGivenNoArguments()
                {
                    this._basicAuthClient.Users().GetAllUsers();
                }

                [Fact]
                public void UsesGETMethod()
                {
                    Assert.Equal(Method.GET, this.Subject.Method);
                }

                [Fact]
                public void RequestsCorrectEndpoint()
                {
                    Assert.Equal("/users", this.Subject.Resource);
                }

                [Fact]
                public void ParsesResults()
                {
                    var response = new RestResponse<User>
                    {
                        Content = this.GetResourceString("Users.list.json")
                    };

                    var json = new JsonDeserializer();
                    var data = json.Deserialize<List<User>>(response);
                    Assert.NotEmpty(data);
                }
            }

            public class WhenGivenAUri : APITest
            {
                public WhenGivenAUri()
                {
                    var uri = new Uri("https://api.github.com/users?since=3");
                    this._basicAuthClient.Users().GetAllUsers(uri);
                }

                [Fact]
                public void UsesGETMethod()
                {
                    Assert.Equal(Method.GET, this.Subject.Method);
                }

                [Fact]
                public void RequestsCorrectEndpoint()
                {
                    Assert.Equal("/users", this.Subject.Resource);
                }

                [Fact]
                public void SetsSinceParameter()
                {
                    var since = this.Subject.Parameters.First(p => p.Name.Equals("since", StringComparison.OrdinalIgnoreCase));
                    Assert.Equal("3", since.Value.ToString());
                }
            }
        }

        public class UpdateUser : APITest
        {
            public UpdateUser()
            {
                var options = new API.UserUpdateOptions();
                options.Name = "David Muto";

                this._tokenAuthClient.Users().UpdateUser(options);
            }

            [Fact]
            public void UsesPATCHMethod()
            {
                Assert.Equal(Method.PATCH, this.Subject.Method);
            }

            [Fact]
            public void RequestsCorrectEndpoint()
            {
                Assert.Equal("/user", this.Subject.Resource);
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

        public class GetEmails : APITest
        {
            public GetEmails()
            {
                this._tokenAuthClient.Users().GetEmails();
            }

            [Fact]
            public void UsesGETMethod()
            {
                Assert.Equal(Method.GET, this.Subject.Method);
            }

            [Fact]
            public void RequestsCorrectEndpoint()
            {
                Assert.Equal("/user/emails", this.Subject.Resource);
            }
         
            [Fact]
            public void ParsesResponse()
            {
                var response = new RestResponse
                {
                    Content = this.GetResourceString("Users.emails.json")
                };

                var json = new JsonDeserializer();
                var emails = json.Deserialize<List<UserEmail>>(response);
                Assert.NotEmpty(emails);

                Assert.Equal("octocat@github.com", emails[0].Email);
                Assert.True(emails[0].Primary);
                Assert.True(emails[0].Verified);
            }
        }

        public class AddEmails : APITest
        {
            public AddEmails()
            {
                this._tokenAuthClient.Users().AddEmails("david.muto@gmail.com", "davidmuto@gmail.com");
            }

            [Fact]
            public void UsesPOSTMethod()
            {
                Assert.Equal(Method.POST, this.Subject.Method);
            }

            [Fact]
            public void RequestsCorrectEndpoint()
            {
                Assert.Equal("/user/emails", this.Subject.Resource);
            }

            [Fact]
            public void SetsRequestBody()
            {
                var body = this.Subject.Parameters.First(p => p.Type.Equals(ParameterType.RequestBody));
                Assert.Equal("[\"david.muto@gmail.com\",\"davidmuto@gmail.com\"]", body.Value.ToString());
            }
        }

        public class DeleteEmails : APITest
        {
            public DeleteEmails()
            {
                this._basicAuthClient.Users().DeleteEmails("david.muto@gmail.com", "davidmuto@gmail.com");
            }

            [Fact]
            public void UsesDELETEMethod()
            {
                Assert.Equal(Method.DELETE, this.Subject.Method);
            }

            [Fact]
            public void RequestsCorrectEndpoint()
            {
                Assert.Equal("/user/emails", this.Subject.Resource);
            }

            [Fact]
            public void SetsRequestBody()
            {
                var body = this.Subject.Parameters.First(p => p.Type.Equals(ParameterType.RequestBody));
                Assert.Equal("[\"david.muto@gmail.com\",\"davidmuto@gmail.com\"]", body.Value.ToString());
            }
        }
    }
}
