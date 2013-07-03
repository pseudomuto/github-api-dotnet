using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class UserPlan : BaseModel
    {
        public string Name { get; set; }

        public long Space { get; set; }

        public int Collaborators { get; set; }

        public int PrivateRepos { get; set; }
    }
}
