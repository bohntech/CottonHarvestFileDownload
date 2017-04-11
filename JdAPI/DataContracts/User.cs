using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    [DataContract]
    public class User : Resource
    {
        [DataMember]
        public String accountName;
        [DataMember]
        public String givenName;
        [DataMember]
        public String familyName;
        [DataMember]
        public String userType;
        [DataMember]
        public String company;

        [IgnoreDataMember]
        public List<DataContracts.Organization> Organizations { get; set; }
    }
}
