using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.Client.Rest
{
    public class OAuthClient
    {
        public String key;
        public String secret;

        public OAuthClient(String key, String secret)
        {
            this.key = key;
            this.secret = secret;
        }

        public String getKey()
        {
            return key;
        }

        public String getSecret()
        {
            return secret;
        }

    }
}
