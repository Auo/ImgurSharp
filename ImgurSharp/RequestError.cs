using Newtonsoft.Json;

namespace ImgurSharp
{
    public class RequestError
    {
        // The Errors can apparently be strings or json objects.
        [JsonProperty("error")]
        public object Error { get; set; }
        [JsonProperty("request")]
        public string Request { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
