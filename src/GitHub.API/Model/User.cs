using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class User : BaseModel
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public Uri AvatarURL { get; set; }

        public string GravatarId { get; set; }

        public string URL { get; set; }

        public Uri HtmlURL { get; set; }
        
        public Uri Blog { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        public bool Hireable { get; set; }

        public string Bio { get; set; }

        public int PublicRepos { get; set; }

        public int TotalPrivateRepos { get; set; }

        public int OwnedPrivateRepos { get; set; }

        public int PublicGists { get; set; }

        public int PrivateGists { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }              
        
        public long DiskUsage { get; set; }

        public int Collaborators { get; set; }

        public UserPlan Plan { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
