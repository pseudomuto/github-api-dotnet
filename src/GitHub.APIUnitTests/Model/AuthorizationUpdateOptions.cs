using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHub.APIUnitTests.Model
{
    public class AuthorizationUpdateOptions
    {
        public class Serialize
        {
            public class WhenNoFieldsNotSupplied
            {
                [Fact]
                public void SerializesToEmptyObject()
                {
                    var result = new API.AuthorizationUpdateOptions().Serialize();
                    Assert.Equal("{}", result);
                }
            }

            public class WhenOptionalFieldNotSupplied
            {
                private string _subject = new API.AuthorizationUpdateOptions
                {
                    Note = "Some note",
                    Scopes = new string[] { "public_repo" },
                    ScopeAction = API.AuthScopeAction.Set
                }.Serialize();

                [Fact]
                public void DoesNotAddEmptyValue()
                {
                    Assert.DoesNotContain("note_url", this._subject);
                }
            }

            public class WhenScopeActionIsNone
            {
                private string _subject = new API.AuthorizationUpdateOptions
                {
                    Note = "Some note",
                    NoteURL = new Uri("http://pseudomuto.com/"),
                    Scopes = new string[] { "public_repo", "gists" }
                }.Serialize();

                [Fact]
                public void SerializesNoteDetails()
                {
                    Assert.Contains("\"note\":", this._subject);
                    Assert.Contains("\"note_url\":", this._subject);
                }

                [Fact]
                public void DoesNotSerializeScopes()
                {
                    Assert.DoesNotContain("scopes", this._subject);
                }
            }

            public class WhenScopeActionIsSet
            {
                private string _subject = new API.AuthorizationUpdateOptions
                {
                    Note = "Some note",
                    NoteURL = new Uri("http://pseudomuto.com/"),
                    Scopes = new string[] { "public_repo", "gists" },
                    ScopeAction = API.AuthScopeAction.Set
                }.Serialize();

                [Fact]
                public void SerializesScopes()
                {
                    Assert.Contains("\"scopes\":", this._subject);
                }
            }

            public class WhenScopeActionIsAdd
            {
                private string _subject = new API.AuthorizationUpdateOptions
                {
                    Note = "Some note",
                    NoteURL = new Uri("http://pseudomuto.com/"),
                    Scopes = new string[] { "public_repo", "gists" },
                    ScopeAction = API.AuthScopeAction.Add
                }.Serialize();

                [Fact]
                public void SerializesScopes()
                {
                    Assert.Contains("\"add_scopes\":", this._subject);
                }
            }

            public class WhenScopeActionIsRemove
            {
                private string _subject = new API.AuthorizationUpdateOptions
                {
                    Note = "Some note",
                    NoteURL = new Uri("http://pseudomuto.com/"),
                    Scopes = new string[] { "public_repo", "gists" },
                    ScopeAction = API.AuthScopeAction.Remove
                }.Serialize();

                [Fact]
                public void SerializesScopes()
                {
                    Assert.Contains("\"remove_scopes\":", this._subject);
                }
            }
        }
    }
}
