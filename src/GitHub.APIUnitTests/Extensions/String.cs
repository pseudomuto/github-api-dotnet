using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using GitHub.API;
using System.Text.RegularExpressions;

namespace GitHub.APIUnitTests.Extensions
{
    public class String
    {
        public class Parameterize
        {
            private string _subject = "tHis$ is-MY String__+_  (*&^%$#@!".Parameterize();

            [Fact]
            public void RemovesDuplicateSeparators()
            {
                Assert.DoesNotContain("__", this._subject);
            }

            [Fact]
            public void RetainsKnownSymbols()
            {
                Assert.Contains("_is-my_", this._subject);

                // underscores should be ok too (if not used as the separator)
                var other = "tHis$ is-MY String__+_  (*&^%$#@!".Parameterize("&");
                Assert.Contains("__&_", other);
            }

            [Fact]
            public void ReturnsLoweredString()
            {
                Assert.False(this._subject.Any(c => char.IsUpper(c)));
            }
        }

        public class Underscore
        {
            private string _subject = "__SomeAPISpecial-property_".Underscore();

            [Fact]
            public void ReturnsLoweredString()
            {
                Assert.False(this._subject.Any(c => char.IsUpper(c)));
            }

            [Fact]
            public void RemovesLeadingOrTrailingUnderscores()
            {
                Assert.False(Regex.IsMatch(this._subject, @"^_|_$"));
            }

            [Fact]
            public void ReplacesSymbolsWithUnderscores()
            {
                Assert.Contains("special_property", this._subject);
            }

            [Fact]
            public void SplitsUpAcronymsFollowedByWord()
            {
                Assert.Contains("api_special", this._subject);
            }

            [Fact]
            public void SeparatesWords()
            {
                Assert.Equal("some_api_special_property", this._subject);
            }
        }
    }
}
