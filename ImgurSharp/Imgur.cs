using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImgurSharp
{
    public class Imgur
    {
        #region Properties
        private readonly string clientId;
        private const string baseUrl = "https://api.imgur.com/3/";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of Imgur object
        /// </summary>
        /// <param name="clientId">Id of application, so Imgur knows which app is submitting data</param>
        public Imgur(string clientId)
        {
            this.clientId = clientId;

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new Exception("clientID is not set, please specify");
            }
        }
        #endregion

        #region ApiMethods
        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="imageStream">Stream of image</param>
        /// <param name="name">Name of image</param>
        /// <param name="title">Title of image</param>
        /// <param name="description">Description of image</param>
        /// <returns>ImgurImage object</returns>
        public async Task<ImgurImage> UploadImageAnonymous(Stream imageStream, string name, string title, string description)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                string base64Image = PhotoStreamToBase64(imageStream);

                var jsonData = JsonConvert.SerializeObject(new
                {
                    image = base64Image,
                    name,
                    title,
                    description
                });

                var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(new Uri(baseUrl + "upload"), jsonContent);

                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();

                ImgurRootObject<ImgurImage> imgRoot = JsonConvert.DeserializeObject<ImgurRootObject<ImgurImage>>(content);

                return imgRoot.Data;
            }
        }
        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="url">Url to image http://some.url.to.image.com/image.jpg</param>
        /// <param name="name">Name of image</param>
        /// <param name="title">Title of image</param>
        /// <param name="description">Description of image</param>
        /// <returns>ImgurImage object</returns>
        public async Task<ImgurImage> UploadImageAnonymous(string url, string name, string title, string description)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);
                var formContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("image", url),
                    new KeyValuePair<string, string>("name", name),
                    new KeyValuePair<string, string>("title", title),
                    new KeyValuePair<string, string>("description", description) });
                HttpResponseMessage response = await client.PostAsync(new Uri(baseUrl + "upload"), formContent);
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();
                ImgurRootObject<ImgurImage> imgRoot = JsonConvert.DeserializeObject<ImgurRootObject<ImgurImage>>(content);

                return imgRoot.Data;
            }
        }
        /// <summary>
        /// Deletes Image from Imgur
        /// </summary>
        /// <param name="deleteHash">DeleteHash, attained when creating image</param>
        /// <returns></returns>
        public async Task<bool> DeleteImageAnonymous(string deleteHash)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                HttpResponseMessage response = await client.DeleteAsync(new Uri(baseUrl + "image/" + deleteHash));
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();
                ImgurRootObject<bool> deleteRoot = JsonConvert.DeserializeObject<ImgurRootObject<bool>>(content);

                return deleteRoot.Data;
            }
        }
        /// <summary>
        /// Update Image
        /// </summary>
        /// <param name="deleteHash">DeleteHash of Image, attained when created</param>
        /// <param name="title">New title</param>
        /// <param name="description">New Description</param>
        /// <returns>bool of result</returns>
        public async Task<bool> UpdateImageAnonymous(string deleteHash, string title, string description)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                var formContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("description", description),
                    new KeyValuePair<string, string>("title", title)
                 });

                HttpResponseMessage response = await client.PutAsync(new Uri(baseUrl + "image/" + deleteHash), formContent);
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();
                ImgurRootObject<bool> deleteRoot = JsonConvert.DeserializeObject<ImgurRootObject<bool>>(content);

                return deleteRoot.Data;

            }
        }
        /// <summary>
        /// Creates an Album on Imgur
        /// </summary>
        /// <param name="imageIds">List of string, ImageIds</param>
        /// <param name="title">Title of album</param>
        /// <param name="description">Description of album</param>
        /// <param name="privacy">Privacy level, use NONE for standard</param>
        /// <param name="layout">Layout, use NONE for standard</param>
        /// <param name="coverImageId">Cover image of album, imageId. Should be in the album</param>
        /// <returns>ImgurCreateAlbum which contains deletehash and link</returns>
        public async Task<ImgurCreateAlbum> CreateAlbumAnonymous(IEnumerable<string> imageIds, string title, string description, ImgurAlbumPrivacy privacy, ImgurAlbumLayout layout, string coverImageId)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                var formContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("ids", imageIds.Aggregate((a,b) => a + "," + b)),
                    new KeyValuePair<string, string>("title", title),
                    new KeyValuePair<string, string>("description", description),
                    new KeyValuePair<string, string>("privacy", GetNameFromEnum<ImgurAlbumPrivacy>((int)privacy)),
                    new KeyValuePair<string, string>("layout", GetNameFromEnum<ImgurAlbumLayout>((int)layout)),
                    new KeyValuePair<string, string>("cover", coverImageId),
                 });

                HttpResponseMessage response = await client.PostAsync(new Uri(baseUrl + "album"), formContent);
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();

                ImgurRootObject<ImgurCreateAlbum> createRoot = JsonConvert.DeserializeObject<ImgurRootObject<ImgurCreateAlbum>>(content);

                return createRoot.Data;
            }
        }
        /// <summary>
        /// Updates ImgurAlbum
        /// </summary>
        /// <param name="deleteHash">DeleteHash, obtained at creation</param>
        /// <param name="imageIds">List images in the album</param>
        /// <param name="title">New title</param>
        /// <param name="description">New description</param>
        /// <param name="privacy">New privacy level, use NONE for standard</param>
        /// <param name="layout">New layout, use NONE for standard</param>
        /// <param name="cover">new coverImage, imageId</param>
        /// <returns>bool of result</returns>
        public async Task<bool> UpdateAlbumAnonymous(string deleteHash, IEnumerable<string> imageIds, string title, string description, ImgurAlbumPrivacy privacy, ImgurAlbumLayout layout, string cover)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                var formContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("ids", imageIds.Aggregate((a,b) => a + "," + b)),
                    new KeyValuePair<string, string>("title", title),
                    new KeyValuePair<string, string>("description", description),
                    new KeyValuePair<string, string>("privacy", GetNameFromEnum<ImgurAlbumPrivacy>((int)privacy)),
                    new KeyValuePair<string, string>("layout", GetNameFromEnum<ImgurAlbumLayout>((int)layout)),
                    new KeyValuePair<string, string>("cover", cover),
                 });

                HttpResponseMessage response = await client.PutAsync(new Uri(baseUrl + "album/" + deleteHash), formContent);
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();

                ImgurRootObject<bool> updateRoot = JsonConvert.DeserializeObject<ImgurRootObject<bool>>(content);

                return updateRoot.Data;
            }
        }
        /// <summary>
        /// Deletes album from Imgur
        /// </summary>
        /// <param name="deleteHash">DeleteHash, obtained when creating Album</param>
        /// <returns></returns>
        public async Task<bool> DeleteAlbumAnonymous(string deleteHash)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                HttpResponseMessage response = await client.DeleteAsync(new Uri(baseUrl + "album/" + deleteHash));
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();
                ImgurRootObject<bool> deleteRoot = JsonConvert.DeserializeObject<ImgurRootObject<bool>>(content);

                return deleteRoot.Data;
            }
        }
        /// <summary>
        /// Add images to excisting album
        /// </summary>
        /// <param name="deleteHash">DeleteHash, obtained when creating album</param>
        /// <param name="imageIds">ALL images must be here, imgur will otherwise remove the ones missing</param>
        /// <returns></returns>
        public async Task<bool> AddImagesToAlbumAnonymous(string deleteHash, IEnumerable<string> imageIds)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                var formContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("ids", imageIds.Aggregate((a,b) => a + "," + b))
                });

                HttpResponseMessage response = await client.PostAsync(new Uri(baseUrl + "album/" + deleteHash), formContent);

                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();

                ImgurRootObject<bool> addRoot = JsonConvert.DeserializeObject<ImgurRootObject<bool>>(content);

                return addRoot.Data;
            }
        }
        /// <summary>
        /// Removes images from album
        /// </summary>
        /// <param name="deleteHash">Album DeleteHash, obtained at creation</param>
        /// <param name="imageIds">List of string, imageIds of images to remove from album</param>
        /// <returns></returns>
        public async Task<bool> RemoveImagesFromAlbumAnonymous(string deleteHash, IEnumerable<string> imageIds)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);

                HttpResponseMessage response = await client.DeleteAsync(new Uri(baseUrl + "album/" + deleteHash + "/remove_images?ids=" + imageIds.Aggregate((a, b) => a + "," + b)));
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();
                ImgurRootObject<bool> removeRoot = JsonConvert.DeserializeObject<ImgurRootObject<bool>>(content);

                return removeRoot.Data;
            }
        }
        /// <summary>
        /// Gets an album from Imgur
        /// </summary>
        /// <param name="albumId">Id of Album</param>
        /// <returns></returns>
        public async Task<ImgurAlbum> GetAlbum(string albumId)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);
                HttpResponseMessage response = await client.GetAsync(new Uri(baseUrl + "album/" + albumId));
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();

                ImgurRootObject<ImgurAlbum> albumRoot = JsonConvert.DeserializeObject<ImgurRootObject<ImgurAlbum>>(content);

                return albumRoot.Data;
            }
        }
        /// <summary>
        /// Gets an image from Imgur
        /// </summary>
        /// <param name="imageId">Id of Image</param>
        /// <returns></returns>
        public async Task<ImgurImage> GetImage(string imageId)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHeaders(client);
                HttpResponseMessage response = await client.GetAsync(new Uri(baseUrl + "image/" + imageId));
                await CheckHttpStatusCode(response);
                string content = await response.Content.ReadAsStringAsync();

                ImgurRootObject<ImgurImage> imageRoot = JsonConvert.DeserializeObject<ImgurRootObject<ImgurImage>>(content);

                return imageRoot.Data;
            }
        }
        #endregion

        #region Helpers
        void SetHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Authorization", "Client-ID " + clientId);
        }

        string PhotoStreamToBase64(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byte[] result = memoryStream.ToArray();
                return System.Convert.ToBase64String(result);
            }
        }

        string GetNameFromEnum<T>(int selected) where T : struct
        {
            string value = Enum.GetName(typeof(T), selected).ToLower();

            if (value == "none")
                value = "";

            return value;
        }

        private async Task CheckHttpStatusCode(HttpResponseMessage responseMessage)
        {
            //Imgur StatusCodes
            var content = await responseMessage.Content.ReadAsStringAsync();
            ImgurRootObject<ImgurRequestError> errorRoot = null;

            try
            {
                errorRoot = JsonConvert.DeserializeObject<ImgurRootObject<ImgurRequestError>>(content);
            }
            catch (Exception) { }

            if (errorRoot == null)
                return;

            if ((int)responseMessage.StatusCode / 100 > 2)
            {
                throw new Exception(string.Format(" Error: {0} \n Request: {1} \n Verb: {2} ", errorRoot.Data.Error, errorRoot.Data.Request, errorRoot.Data.Method));
            }

            return;
        }
        #endregion
    }
}