using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ImgurSharp.Test
{


    [TestClass]
    public class AlbumTest
    {
        private static Imgur imgur;
        private const string apiKey = "fake-api-key";
        private const string baseUrl = "https://api.imgur.com/3/";

        [TestMethod]
        public async Task TestCreateAlbum()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(HttpMethod.Post, baseUrl + "album")
                .Respond(HttpStatusCode.OK, MediaTypeNames.Application.Json,
                ResourceRetriever.GetJsonString("responses/create-album-ok.json"));

            imgur = new Imgur(apiKey, mockHttp);

            CreateAlbum album = await imgur.CreateAlbumAnonymous(new string[] { "delete-hash-1" }, "title", "description", AlbumPrivacy.None, AlbumLayout.Blog, "image-id");

            Assert.AreEqual("some-delete-hash", album.DeleteHash);
            Assert.AreEqual("some-id", album.Id);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (imgur != null)
            {
                imgur.Dispose();
                imgur = null;
            }
        }
    }
}
