using GitHub.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.APIUnitTests.Fakes
{
    public class FakeModel : BaseModel
    {
        public string SomeName { get; set; }

        public int ClientId { get; set; }
    }
}
