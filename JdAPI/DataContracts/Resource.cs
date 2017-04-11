using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    [DataContract]
    public class Resource
    {
        [DataMember]
        public List<Link> links;

        [DataMember]
        public String id;
    }

    [DataContract]
    public class PostResource
    {
        [DataMember]
        public List<PostLink> links;
    }

    [DataContract]
    public class PostLink
    {
        [DataMember]
        public string rel;

        [DataMember]
        public string uri;
    }
}
