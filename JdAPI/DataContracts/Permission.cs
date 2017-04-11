using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    public class Permissions : Resource
    {
        [DataMember]
        public List<Permission> permissions;
    }

    public class Permission 
    {        
        [DataMember(Name = "type")]        
        public string type;

        [DataMember]        
        public string permissionId;

        [DataMember]        
        public string status;

        [DataMember]
        public List<Link> links;
    }
}
