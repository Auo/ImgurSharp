using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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
            try
            {
                //Image
                ImgurImage urlImage = await imgur.UploadImageAnonymous("https://assets-cdn.github.com/images/modules/logos_page/GitHub-Mark.png", "name", "title", "description");

                byte[] buff = File.ReadAllBytes("vs-icon.png");
                MemoryStream ms = new MemoryStream(buff);

                ImgurImage streamImage = await imgur.UploadImageAnonymous(ms, "name", "title", "description");
                bool updated = await imgur.UpdateImageAnonymous(streamImage.Deletehash, "updated title", "a new description");

                ImgurImage getImage = await imgur.GetImage(streamImage.Id);

                //Album
                ImgurCreateAlbum createdAlbum = await imgur.CreateAlbumAnonymous(new string[] { streamImage.Id }, "album title", "album description", ImgurAlbumPrivacy.Public, ImgurAlbumLayout.Horizontal, streamImage.Id);

                bool result = await imgur.UpdateAlbumAnonymous(createdAlbum.DeleteHash, new string[] { streamImage.Id, urlImage.Id }, "updated album title", "update album description", ImgurAlbumPrivacy.Hidden, ImgurAlbumLayout.Blog, urlImage.Id);
                bool addImagesResult = await imgur.AddImagesToAlbumAnonymous(createdAlbum.DeleteHash, new string[] { streamImage.Id, urlImage.Id });
                bool removeImagesResult = await imgur.RemoveImagesFromAlbumAnonymous(createdAlbum.DeleteHash, new string[] { urlImage.Id });
                ImgurAlbum album = await imgur.GetAlbum(createdAlbum.Id);



                //Delete
                bool deleteAlbum = await imgur.DeleteAlbumAnonymous(createdAlbum.DeleteHash);
                bool deletedUrlImage = await imgur.DeleteImageAnonymous(urlImage.Deletehash);
                bool deletedStreamImage = await imgur.DeleteImageAnonymous(streamImage.Deletehash);
            }
            catch (Exception e)
            {
                System.Console.Write(e.Message);
            }
        }
    }
}
