using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImgurSharp
{
    public class ImgurAlbum
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public object Description { get; set; }
        [JsonProperty("datetime")]
        public int Datetime { get; set; }
        [JsonProperty("cover")]
        public string Cover { get; set; }
        [JsonProperty("account_url")]
        public string AccountUrl { get; set; }
        [JsonProperty("privacy")]
        public string Privacy { get; set; }
        [JsonProperty("layout")]
        public string Layout { get; set; }
        [JsonProperty("views")]
        public int Views { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("images_count")]
        public int ImagesCount { get; set; }
        [JsonProperty("images")]
        public List<ImgurImage> Images { get; set; }
    }
}
