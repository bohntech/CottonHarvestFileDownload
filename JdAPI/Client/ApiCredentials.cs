using JdAPI.Client.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.Client
{
    public abstract class ApiCredentials
    {
        public static OAuthClient CLIENT = new OAuthClient("", "");
        public static OAuthToken TOKEN = new OAuthToken("", "");
    }
}
