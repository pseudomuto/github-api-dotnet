using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class UserEmail : BaseModel
    {
        public string Email { get; set; }

        public bool Verified { get; set; }

        public bool Primary { get; set; }
    }
}
