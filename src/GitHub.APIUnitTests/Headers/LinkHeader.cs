using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests.Headers
{
    public class LinkHeader
    {
        public class Constructor
        {
            [Fact]
            public void RequiresValue()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new API.LinkHeader(string.Empty);
                });
            }

            [Fact]
            public void ParsesValues()
            {
                var input = "<https://api.github.com/user/repos?page=3&per_page=100>; rel=\"next\", <https://api.github.com/user/repos?page=50&per_page=100>; rel=\"last\"";
                var header = new API.LinkHeader(input);
                
                Assert.Equal("https://api.github.com/user/repos?page=3&per_page=100", header.Next().ToString());
                Assert.Equal("https://api.github.com/user/repos?page=50&per_page=100", header.Last().ToString());
                Assert.Null(header.Prev());
                Assert.Null(header.First());
            }
        }
    }
}
