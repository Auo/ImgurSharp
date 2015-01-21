using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
