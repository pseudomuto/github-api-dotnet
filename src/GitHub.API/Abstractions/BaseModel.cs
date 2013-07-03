using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public abstract class BaseModel
    {
        protected BaseModel()
        {
        }

        public virtual string Serialize()
        {
            var props = new Dictionary<string, object>();

            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                props.Add(
                        this.GetPropertyName(prop.Name),
                        prop.GetValue(this)
                    );
            }

            var serializer = new JsonSerializer();
            return serializer.Serialize(props);
        }

        protected virtual string GetPropertyName(string name)
        {
            return name.Underscore().Parameterize();
        }
    }
}
