using System;
using System.IO;

namespace ImgurSharp.Console
{
    class Program
    {
        static Imgur imgur;

        static void Main(string[] args)
        {
            imgur = new Imgur("YourKeyHere");
            UploadImage();
            System.Console.ReadLine();
        }

        private static async void UploadImage()
        {
            ImgurImage urlImage = null;
            ImgurImage streamImage = null;

            ImgurCreateAlbum album = null;
            try
            {
                //Image
                 urlImage = await imgur.UploadImageAnonymous("https://github.com/fluidicon.png", "name", "title", "description");

                byte[] buff = File.ReadAllBytes("vs-icon.png");
                using (MemoryStream ms = new MemoryStream(buff))
                {
                    streamImage = await imgur.UploadImageAnonymous(ms, "name", "title", "description");
                }

                bool updated = await imgur.UpdateImageAnonymous(streamImage.Deletehash, "updated title", "a new description");

                ImgurImage getImage = await imgur.GetImage(streamImage.Id);

                //Album
                 album = await imgur.CreateAlbumAnonymous(new string[] { streamImage.Id }, "album title", "album description", ImgurAlbumPrivacy.Public, ImgurAlbumLayout.Horizontal, streamImage.Id);

                bool result = await imgur.UpdateAlbumAnonymous(album.DeleteHash, new string[] { streamImage.Id, urlImage.Id }, "updated album title", "update album description", ImgurAlbumPrivacy.Hidden, ImgurAlbumLayout.Blog, urlImage.Id);
                bool addImagesResult = await imgur.AddImagesToAlbumAnonymous(album.DeleteHash, new string[] { streamImage.Id, urlImage.Id });
                bool removeImagesResult = await imgur.RemoveImagesFromAlbumAnonymous(album.DeleteHash, new string[] { urlImage.Id });
                ImgurAlbum getAlbum = await imgur.GetAlbum(album.Id);
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            } finally
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
        }
    }
}
