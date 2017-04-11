using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace JdAPI.DataContracts
{
    [DataContract]
    public class Link
    {
        [DataMember]
        public string rel;

        [DataMember]
        public string uri;

        [DataMember]
        public String followable;
    }
}
