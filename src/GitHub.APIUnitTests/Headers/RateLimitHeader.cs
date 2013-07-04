using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests.Headers
{
    public class RateLimitHeader
    {
        public class Constructor 
        {
            private API.RateLimitHeader _subject;

            public Constructor()
            {
                var response = new RestResponse();
                response.Headers.Add(new Parameter { Name = "X-RateLimit-Limit", Value = "60", Type = ParameterType.HttpHeader });
                response.Headers.Add(new Parameter { Name = "X-RateLimit-Remaining", Value = "54", Type = ParameterType.HttpHeader });
                response.Headers.Add(new Parameter { Name = "X-RateLimit-Reset", Value = "1372700873", Type = ParameterType.HttpHeader });

                this._subject = new API.RateLimitHeader(response);
            }
            
            [Fact]
            public void RequiresRestResponseToNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => new API.RateLimitHeader(null));
            }

            [Fact]
            public void ParsesLimitFromHeader()
            {
                Assert.Equal(60, this._subject.Limit);
            }

            [Fact]
            public void ParsesRemainingFromHeader()
            {
                Assert.Equal(54, this._subject.Remaining);
            }

            [Fact]
            public void ParsesResetFromHeaders()
            {
                // Used http://www.epochconverter.com/ to determine expected response
                var expectedDate = new DateTime(2013, 7, 1, 13, 47, 53);
                Assert.Equal(expectedDate, this._subject.Reset);
            }
        }
    }
}
