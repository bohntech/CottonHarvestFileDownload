using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonHarvestDataTransferApp.Data
{
    public class JDeere
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string WellKnown { get; set; }
        public string ServerUrl { get; set; }
        public string CallbackUrl { get; set; }
        public Token AccessToken { get; set; }
        public string Scopes { get; set; }
        public string State { get; set; }
        public string APIURL { get; set; }
    }

    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}
