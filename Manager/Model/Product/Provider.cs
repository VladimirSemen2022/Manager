using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Model
{
    class Provider
    {
        public int ProviderId { get; set; }
        public string ProviderFullName { get; set; }
        public string ProviderShortName { get; set; }
        public string Address { get; set; }

        public Provider(int ProviderId=0, string ProviderFullName="", string ProviderShortName="", string Address="")
        {
            this.ProviderId = ProviderId;
            this.ProviderFullName = ProviderFullName;
            this.ProviderShortName = ProviderShortName;
            this.Address = Address;
        }

        public override string ToString()
        {
            return $"Name [{ProviderFullName}], Address [{Address}]";
        }
    }
}
