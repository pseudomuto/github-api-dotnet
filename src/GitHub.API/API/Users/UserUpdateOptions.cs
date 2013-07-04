using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.API
{
    public class UserUpdateOptions : BaseModel
    {
        private bool _serializeEmptyFields;

        public string Name { get; set; }

        public string Email { get; set; }

        public Uri Blog { get; set; }

        public string Company { get; set; }

        public string Location { get; set; }

        public bool Hireable { get; set; }

        public string Bio { get; set; }

        public UserUpdateOptions(bool serializeEmptyFields = false)
        {
            this._serializeEmptyFields = serializeEmptyFields;
        }

        public override string Serialize()
        {
            if(this._serializeEmptyFields) return base.Serialize();

            var props = new Dictionary<string, object>();
            this.AddIfNeeded(props, "Name", this.Name);
            this.AddIfNeeded(props, "Email", this.Email);
            this.AddIfNeeded(props, "Company", this.Company);
            this.AddIfNeeded(props, "Location", this.Location);
            this.AddIfNeeded(props, "Bio", this.Bio);
                        
            props.Add(this.GetPropertyName("Hireable"), this.Hireable);

            if (this.Blog != null)
            {
                props.Add(this.GetPropertyName("Blog"), this.Blog);
            }

            var json = new JsonSerializer();
            return json.Serialize(props);
        }

        private void AddIfNeeded(IDictionary<string, object> props, string name, string value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                props.Add(this.GetPropertyName(name), value);
            }
        }
    }
}
