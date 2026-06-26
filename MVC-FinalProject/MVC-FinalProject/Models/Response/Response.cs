using Newtonsoft.Json;

namespace MVC_FinalProject.Models.Response
{
    public class Response
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
