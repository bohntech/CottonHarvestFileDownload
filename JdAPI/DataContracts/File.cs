using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.DataContracts
{
    [DataContract]
    public class File : Resource
    {

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "@type")]
        public string aType;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string createdTime;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string modifiedTime;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long nativeSize;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string source;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Boolean transferPending;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string visibleViaShare;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Boolean shared;

        // [DataMember]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //internal Boolean _new;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string status;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string invalidFileReasonText;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String archived;

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String success;
    }
}
