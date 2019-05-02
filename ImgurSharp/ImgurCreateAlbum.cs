using Newtonsoft.Json;

namespace ImgurSharp
{
    public class ImgurCreateAlbum
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("deletehash")]
        public string DeleteHash { get; set; }
    }
}
