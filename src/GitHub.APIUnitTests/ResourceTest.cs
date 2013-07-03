using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.APIUnitTests
{
    public abstract class ResourceTest
    {
        protected string GetResourceString(string name, bool fullyQualified = false)
        {
            if (!fullyQualified)
            {
                name = this.CreateResourceName(name);
            }

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

            try
            {
                using (var reader = new StreamReader(stream))
                {
                    stream = null;
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        protected virtual string CreateResourceName(string name)
        {
            var asmName = Assembly.GetExecutingAssembly().GetName();
            return string.Concat(asmName.Name, ".Resources.", name);
        }
    }
}
