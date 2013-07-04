using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests.Model
{
    public class UserUpdateOptions
    {
        public class Serialize
        {
            public class WhenSerializeEmptyFieldsIsTrue
            {
                private string _subject;

                public WhenSerializeEmptyFieldsIsTrue()
                {
                    var options = new API.UserUpdateOptions(true);
                    options.Name = "David Muto";

                    this._subject = options.Serialize();
                }

                [Fact]
                public void SerializesAllFields()
                {
                    Assert.True(this.HasProp("name"));
                    Assert.True(this.HasProp("email"));
                    Assert.True(this.HasProp("blog"));
                    Assert.True(this.HasProp("company"));
                    Assert.True(this.HasProp("location"));
                    Assert.True(this.HasProp("hireable"));
                    Assert.True(this.HasProp("bio"));
                }

                private bool HasProp(string name)
                {
                    return this._subject.Contains(string.Format("\"{0}\":", name));
                }
            }

            public class WhenSerializeEmptyFieldsIsFalse
            {
                private string _subject;

                public WhenSerializeEmptyFieldsIsFalse()
                {
                    var options = new API.UserUpdateOptions(false);
                    options.Name = "David Muto";

                    this._subject = options.Serialize();
                }

                [Fact]
                public void SerializesOnlySuppliedFields()
                {
                    Assert.True(this.HasProp("name"));
                    Assert.True(this.HasProp("hireable"));

                    Assert.False(this.HasProp("email"));
                    Assert.False(this.HasProp("blog"));
                    Assert.False(this.HasProp("company"));
                    Assert.False(this.HasProp("location"));                    
                    Assert.False(this.HasProp("bio"));
                }

                private bool HasProp(string name)
                {
                    return this._subject.Contains(string.Format("\"{0}\":", name));
                }
            }
        }
    }
}
