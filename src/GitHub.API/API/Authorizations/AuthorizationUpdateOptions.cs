using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public enum AuthScopeAction
    {
        None,
        Set,
        Add,
        Remove
    }

    public class AuthorizationUpdateOptions : BaseModel
    {
        public string[] Scopes { get; set; }

        public string Note { get; set; }

        public Uri NoteURL { get; set; }

        public AuthScopeAction ScopeAction { get; set; }

        public override string Serialize()
        {
            var props = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(this.Note))
            {
                props.Add(this.GetPropertyName("Note"), this.Note);
            }

            if (this.NoteURL != null)
            {
                props.Add(this.GetPropertyName("NoteURL"), this.NoteURL);
            }

            var fieldName = string.Empty;

            switch(this.ScopeAction)
            {
                case AuthScopeAction.Set:
                    fieldName = this.GetPropertyName("Scopes");
                    break;
                case AuthScopeAction.Add:
                    fieldName = this.GetPropertyName("AddScopes");
                    break;
                case AuthScopeAction.Remove:
                    fieldName = this.GetPropertyName("RemoveScopes");
                    break;
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                props.Add(fieldName, this.Scopes);
            }

            return new JsonSerializer().Serialize(props);
        }
    }
}
