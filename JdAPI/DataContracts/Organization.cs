using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    [DataContract]
    public class Organization : Resource
    {
        [DataMember]
        public string name;

        [DataMember]
        public string type;

        [DataMember]
        public string accountId;

        [DataMember]
        public Boolean member;
      
    }
}
