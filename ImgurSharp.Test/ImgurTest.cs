using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ImgurSharp.Test
{
    [TestClass]
    public class ImgurTest
    {
        private static Imgur imgur;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            string apiKey = ConfigurationManager
                .OpenExeConfiguration(Assembly.GetExecutingAssembly().Location)
                .AppSettings
                .Settings["Api-Key"].Value;

            imgur = new Imgur(apiKey);
        }

        [TestMethod]
        public async Task TestGetAlbum()
        {
            // https://imgur.com/gallery/YuQ8OrD
            Album album = await imgur.GetAlbum("YuQ8OrD");
            Assert.IsNotNull(album);
        }

        [TestMethod]
        public async Task TestGetImage()
        {
            //https://i.imgur.com/y3NI9PF.jpg
            Image image = await imgur.GetImage("y3NI9PF");
            Assert.IsNotNull(image);
        }

        [TestMethod]
        public async Task TestImgur()
        {
            string error = null;
            Image urlImage = null;
            Image streamImage = null;
            CreateAlbum album = null;

            try
            {
                urlImage = await imgur.UploadImageAnonymous("https://github.com/fluidicon.png", "name", "title", "description");

                using (MemoryStream ms = new MemoryStream(File.ReadAllBytes("vs-icon.png")))
                {
                    streamImage = await imgur.UploadImageAnonymous(ms, "name", "title", "description");
                }

                bool updated = await imgur.UpdateImageAnonymous(streamImage.Deletehash, "updated title", "a new description");

                //Album
                album = await imgur.CreateAlbumAnonymous(new string[] { streamImage.Deletehash }, "album title", "album description", AlbumPrivacy.Public, AlbumLayout.Horizontal, streamImage.Id);

                bool result = await imgur.UpdateAlbumAnonymous(album.DeleteHash, new string[] { streamImage.Deletehash, urlImage.Deletehash }, "updated album title", "update album description", AlbumPrivacy.Hidden, AlbumLayout.Blog, streamImage.Id);
                bool addImagesResult = await imgur.AddImagesToAlbumAnonymous(album.DeleteHash, new string[] { streamImage.Deletehash, urlImage.Deletehash });
                bool removeImagesResult = await imgur.RemoveImagesFromAlbumAnonymous(album.DeleteHash, new string[] { urlImage.Id });
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            finally
            {
                if (urlImage != null)
                {
                    await imgur.DeleteImageAnonymous(urlImage.Deletehash);
                }

                if (streamImage != null)
                {
                    await imgur.DeleteImageAnonymous(streamImage.Deletehash);
                }

                if (album != null)
                {
                    await imgur.DeleteAlbumAnonymous(album.DeleteHash);
                }
            }

            Assert.IsNull(error, error);
        }
    }
}
