using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class Authorization : BaseModel
    {
        public long Id { get; set; }

        public Uri Url { get; set; }

        public List<string> Scopes { get; set; }

        public string Token { get; set; }

        public string Note { get; set; }

        public Uri NoteUrl { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }     


    }
}
