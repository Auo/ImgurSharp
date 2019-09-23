using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ImgurSharp.Test
{
    [TestClass]
    public class ImageTest
    {
        private static Imgur imgur;
        private const string apiKey = "fake-api-key";
        private const string baseUrl = "https://api.imgur.com/3/";

        [TestMethod]
        public async Task TestGetImage()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(baseUrl + "image/some-image-id")
                .Respond(MediaTypeNames.Application.Json,
                ResourceRetriever.GetJsonString("responses/get-image-ok.json"));

            imgur = new Imgur(apiKey, mockHttp);

            Image image = await imgur.GetImage("some-image-id");
            Assert.AreEqual("some-image-id", image.Id);
        }

        [TestMethod]
        public async Task TestGetNotExistingImage()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(baseUrl + "image/non-existing-id")
                .Respond(HttpStatusCode.NotFound, MediaTypeNames.Application.Json,
                ResourceRetriever.GetJsonString("responses/image-not-found.json"));

            imgur = new Imgur(apiKey, mockHttp);

            try
            {
                Image img = await imgur.GetImage("non-existing-id");
                Assert.Fail();
            }
            catch (ResponseException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Unable to find an image with the id"));
            }
        }

        [TestMethod]
        public async Task TestUploadImageStream()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(HttpMethod.Post, baseUrl + "upload")
                .Respond(HttpStatusCode.OK, MediaTypeNames.Application.Json,
                ResourceRetriever.GetJsonString("responses/upload-image-stream-ok.json"));

            imgur = new Imgur(apiKey, mockHttp);

            Image image = await imgur.UploadImageAnonymous(ResourceRetriever.GetStream("images/vs-icon.png"), "name", "title", "description");

            Assert.AreEqual("some-id", image.Id);
            Assert.AreEqual("title", image.Title);
            Assert.AreEqual("some-delete-hash", image.Deletehash);
            Assert.AreEqual("https://i.imgur.com/some-id.png", image.Link);
            Assert.AreEqual("image/png", image.Type);
        }

        [TestMethod]
        public async Task TestDeleteImage()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(HttpMethod.Post, baseUrl + "image/some-delete-hash")
                .Respond(HttpStatusCode.OK, MediaTypeNames.Application.Json,
                ResourceRetriever.GetJsonString("responses/delete-image-ok.json"));

            imgur = new Imgur(apiKey, mockHttp);

            bool success = await imgur.DeleteImageAnonymous("some-delete-hash");

            Assert.IsTrue(success);
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
