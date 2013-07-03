using GitHub.API;
using RestSharp;
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
                public void LeavesAuthenticatorSetToNull()
                {
                    Assert.Null(this._subject.Authenticator);
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
                public void SetAuthenticatorToHttpBasic()
                {
                    Assert.IsAssignableFrom(typeof(HttpBasicAuthenticator), this._subject.Authenticator);                    
                }

                [Fact]
                public void SetsAuthTypeToBasic()
                {
                    Assert.Equal(AuthType.Basic, this._subject.AuthType);
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
                public void SetsAuthenticatorToTokenAuthenticator()
                {
                    Assert.IsAssignableFrom(typeof(OAuth2AuthorizationRequestHeaderAuthenticator), this._subject.Authenticator);
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
            public void SetsAuthenticatorToTokenAuthenticator()
            {
                Assert.IsAssignableFrom(typeof(OAuth2AuthorizationRequestHeaderAuthenticator), this._subject.Authenticator);
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
            public void SetAuthenticatorToHttpBasic()
            {
                Assert.IsAssignableFrom(typeof(HttpBasicAuthenticator), this._subject.Authenticator);
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
