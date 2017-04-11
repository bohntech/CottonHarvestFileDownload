using Hammock.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JdAPI.DataContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.Client.Rest
{
    public class CollectionPageDeserializer
    {

        public CollectionPage<T> deserialize<T>(string s)
        {
            List<T> objects = new List<T>();
            List<Link> links = new List<Link>();
            int total = 0;


            Dictionary<string, string> nodes = new Dictionary<string, string>();

            dynamic dynObj = JsonConvert.DeserializeObject(s);

            var jObj = (JObject)dynObj;

            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jObj.ToString());

            foreach (KeyValuePair<string, object> item in dict)
            {
                if ("links" == item.Key)
                {
                    links.AddRange(JsonConvert.DeserializeObject<List<Link>>(item.Value.ToString()));
                }
                else if ("total" == item.Key)
                {
                    total = JsonConvert.DeserializeObject<int>(item.Value.ToString());
                }
                else if ("values" == item.Key)
                {
                    //List<SampleApp.Sources.generated.v3.File> f = JsonConvert.DeserializeObject<List<SampleApp.Sources.generated.v3.File>>(item.Value.ToString());
                    objects = JsonConvert.DeserializeObject<List<T>>(item.Value.ToString());
                    Console.WriteLine("done");
                }
            }

            //return objects;



            return new CollectionPage<T>(objects,
                                              getSelf(links),
                                              getNextPage(links),
                                              getPreviousPage(links),
                                              total);

            //throw ctxt.wrongTokenException(jp, START_OBJECT, "Input JSON could not be deserialized to CollectionPage");
        }

        private Uri getSelf(List<Link> links)
        {
            return findLinkUriByRel(links, "self");
        }

        private Uri findLinkUriByRel(List<Link> links, String rel)
        {
            foreach (Link link in links)
            {
                if (rel == link.rel)
                {
                    return new Uri(link.uri);
                }
            }
            return null;
        }

        private Uri getPreviousPage(List<Link> links)
        {
            return findLinkUriByRel(links, "previousPage");
        }

        private Uri getNextPage(List<Link> links)
        {
            return findLinkUriByRel(links, "nextPage");
        }

    }
}
