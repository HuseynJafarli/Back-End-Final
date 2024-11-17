using Newtonsoft.Json;

namespace YouPlay.MVC.ViewModels
{
    public record TokenResponseVM
    {
        [JsonProperty("accessToken")] 
        public string AccessToken { get; set; }

        [JsonProperty("expireDate")]
        public DateTime ExpireDate { get; set; }
    }


}
