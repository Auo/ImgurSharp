using Newtonsoft.Json;

namespace ImgurSharp
{
    public class ImgurRootObject<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
