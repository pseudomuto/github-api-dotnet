﻿using GitHub.APIUnitTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests.Model
{
    public class BaseModel
    {
        public class Serialize
        {
            private string _subject = new FakeModel().Serialize();

            [Fact]
            public void CreatesObject()
            {
                Assert.NotNull(this._subject);
            }

            [Fact]
            public void UsesRubyStyleNamesForProperties()
            {
                Assert.Contains("\"some_name\":", this._subject);
                Assert.Contains("\"client_id\":", this._subject);
            }
        }
    }
}
