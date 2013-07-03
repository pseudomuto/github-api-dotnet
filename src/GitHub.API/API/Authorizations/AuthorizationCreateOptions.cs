using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class AuthorizationCreateOptions : BaseModel
    {
        public string[] Scopes { get; set; }

        public string Note { get; set; }

        public Uri NoteURL { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
