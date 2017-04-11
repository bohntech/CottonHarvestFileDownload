using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    [DataContract]
    public class Partnership : Resource
    {        
        [IgnoreDataMember]
        public Organization FromOrg { get; set; }

        [IgnoreDataMember]
        public Organization ToOrg { get; set; }

        [IgnoreDataMember]
        public Resource ContactInvite { get; set; }
             
        [IgnoreDataMember]
        public List<DataContracts.File> SharedFiles { get; set; }

        [IgnoreDataMember]
        public int TotalFileCount { get; set; }

        /*[IgnoreDataMember]
        public List<DataContracts.Permission> Permissions { get; set; }*/

    }
}
