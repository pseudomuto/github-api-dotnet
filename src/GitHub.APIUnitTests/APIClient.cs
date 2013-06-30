using GitHub.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests
{
    public class APIClient
    {
        public class Constructor
        {
            public class WhenGivenNoParameters
            {
                private API.APIClient _subject = new API.APIClient();

                [Fact]
                public void SetsAuthTypeToNone()
                {
                    Assert.Equal(AuthType.None, this._subject.AuthType);
                }

                [Fact]
                public void LeavesAuthValueAsNull()
                {
                    Assert.Null(this._subject.AuthValue);
                }
            }

            public class WhenGivenUsernameAndPassword
            {
                private API.APIClient _subject = new API.APIClient("username", "password");

                [Fact]
                public void RequiresUsernameToBeValid()
                {
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        new API.APIClient(string.Empty, "password");
                    });
                }

                [Fact]
                public void RequiresPasswordToBeValid()
                {
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        new API.APIClient("username", null);
                    });
                }

                [Fact]
                public void SetsAuthTypeToBasic()
                {
                    Assert.Equal(AuthType.Basic, this._subject.AuthType);
                }

                [Fact]
                public void SetsAuthValueToBase64String()
                {
                    var expected = Convert.ToBase64String(Encoding.UTF8.GetBytes("username:password"));
                    Assert.Equal(expected, this._subject.AuthValue);
                }
            }

            public class WhenGivenAuthToken
            {
                private API.APIClient _subject = new API.APIClient("myAuthToken");

                [Fact]
                public void RequiresTokenToBeValid()
                {
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        new API.APIClient("");
                    });
                }

                [Fact]
                public void SetsAuthTypeToToken()
                {
                    Assert.Equal(AuthType.Token, this._subject.AuthType);
                }

                [Fact]
                public void SetsAuthValueToToken()
                {
                    Assert.Equal("myAuthToken", this._subject.AuthValue);
                }
            }
        }

        public class WithAuthToken
        {
            private API.APIClient _subject = new API.APIClient().WithAuthToken("myAuthToken");

            [Fact]
            public void RequiresTokenToBeValid()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    this._subject.WithAuthToken(string.Empty);
                });
            }

            [Fact]
            public void SetsAuthTypeToToken()
            {
                Assert.Equal(AuthType.Token, this._subject.AuthType);
            }

            [Fact]
            public void SetsAuthValueToToken()
            {
                Assert.Equal("myAuthToken", this._subject.AuthValue);
            }

            [Fact]
            public void ReturnsReferenceToSelf()
            {
                var returnedInstance = this._subject.WithAuthToken("token");
                Assert.Same(this._subject, returnedInstance);
            }
        }

        public class WithCredentials
        {
            private API.APIClient _subject = new API.APIClient().WithCredentials("username", "password");

            [Fact]
            public void RequiresUsernameToBeValid()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    this._subject.WithCredentials(string.Empty, "password");
                });
            }

            [Fact]
            public void RequiresPasswordToBeValid()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    this._subject.WithCredentials("username", null);
                });
            }

            [Fact]
            public void SetsAuthTypeToBasic()
            {
                Assert.Equal(AuthType.Basic, this._subject.AuthType);
            }

            [Fact]
            public void SetsAuthValueToBase64String()
            {
                var expected = Convert.ToBase64String(Encoding.UTF8.GetBytes("username:password"));
                Assert.Equal(expected, this._subject.AuthValue);
            }

            [Fact]
            public void ReturnsReferenceToSelf()
            {
                var returnedInstance = this._subject.WithCredentials("user", "pass");
                Assert.Same(this._subject, returnedInstance);
            }
        }
    }
}
