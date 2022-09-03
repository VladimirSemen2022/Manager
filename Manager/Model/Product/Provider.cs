using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Model
{
    class Provider
    {
        public int Id { get; set; }
        //public int ProviderId { get; set; }
        public string ProviderFullName { get; set; }
        public string ProviderShortName { get; set; }
        public string Address { get; set; }

        public Provider()
        {
            this.Id = 0;
            //this.ProviderId = 0;
            this.ProviderFullName = string.Empty;
            this.ProviderShortName = string.Empty;
            this.Address = string.Empty;
        }
        public Provider(int Id=0, string ProviderFullName="", string ProviderShortName="", string Address="")
        {
            this.Id = Id;
            this.ProviderFullName = ProviderFullName;
            this.ProviderShortName = ProviderShortName;
            this.Address = Address;
        }

        public override string ToString()
        {
            return $"Id [{Id}] Name [{ProviderFullName}], Address [{Address}]";
        }
    }
}
