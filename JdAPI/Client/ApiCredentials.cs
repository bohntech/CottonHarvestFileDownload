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
        //public static OAuthClient CLIENT = new OAuthClient("", "");
        //public static OAuthToken TOKEN = new OAuthToken("", "");

        public static string APP_ID = "";
        public static string APP_KEY = "";
        public static string ACCESS_TOKEN = "";
        public static string REFRESH_TOKEN = "";
    }

    public abstract class Secrets
    {
        public static string ClientId = "";
        public static string ClientSecret = "";
        public static string Scopes = "";        
    }
}
