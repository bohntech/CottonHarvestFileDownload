using JdAPI.Client.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    public class ETag
    {
        public DateTime CreatedDate;
        public string Tag;
    }

    public class TagResource<TResource>
    {
        public string Tag { get; set; }
        public List<TResource> Resources { get; set; }
    }
}
