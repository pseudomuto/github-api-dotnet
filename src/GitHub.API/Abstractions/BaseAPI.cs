using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public abstract class BaseAPI
    {
        public APIClient APIClient { get; private set; }

        protected BaseAPI(APIClient apiClientInstance)
        {
            if (apiClientInstance == null) throw new ArgumentNullException("apiClientInstance");
            this.APIClient = apiClientInstance;
        }
    }
}
