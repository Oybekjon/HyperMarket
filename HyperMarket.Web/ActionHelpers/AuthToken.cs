using Newtonsoft.Json;

namespace HyperMarket.Web.ActionHelpers
{
    public class AuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}