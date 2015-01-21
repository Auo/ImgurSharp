using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgurSharp
{
    public class ImgurRequestError
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("request")]
        public string Request { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
