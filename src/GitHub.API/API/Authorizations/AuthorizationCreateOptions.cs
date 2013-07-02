using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class AuthorizationCreateOptions
    {
        public string[] scopes { get; set; }

        public string note { get; set; }

        public Uri note_url { get; set; }

        public string client_id { get; set; }

        public string client_secret { get; set; }
    }
}
