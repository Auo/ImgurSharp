# ImgurSharp

C# wrapper for imgur.com api v3, ONLY anonymously.
* Upload, edit and delete images
* Create, edit and delete albums

You'll need to register on Imgur for a ApplicationId / Client-ID. [Imgur api](https://apidocs.imgur.com/)

### Create
```csharp
using ImgurSharp;

Imgur imgur = new Imgur("YOUR APPLICATION ID");
```
 
### Image
#### Upload from url
 Store DeleteHash from creation if you want to edit/delete image
```csharp
ImgurImage image = await imgur.UploadImageAnonymous("https://www.link.to.your.image.on.some.url.com/image.png", "name", "title", "description");
``` 
#### Upload from Stream
 Store DeleteHash from creation if you want to edit/delete image
```csharp
byte[] buff = File.ReadAllBytes("someLocalFile.jpg");
MemoryStream ms = new MemoryStream(buff);

ImgurImage image = await imgur.UploadImageAnonymous(ms, "name", "title", "description");
``` 

#### Update
```csharp
bool updated = await imgur.UpdateImageAnonymous(image.Deletehash, "updated title", "a new description");
``` 
#### Delete
```csharp
bool deleted = await imgur.DeleteImageAnonymous(image.Deletehash);
``` 
#### Get 
Will NOT return deleteHash
```csharp
ImgurImage image = await imgur.GetImage("imageId");
``` 

### Album

#### Create
 Store DeleteHash from creation if you want to edit/delete album
```csharp
ImgurCreateAlbum createdAlbum = await imgur.CreateAlbumAnonymous(new string[] { "imageId#1","imageId#2" }, "album title", "album description", ImgurAlbumPrivacy.Public, ImgurAlbumLayout.Horizontal, "imageId#1");
``` 

#### Update
```csharp
bool result = await imgur.UpdateAlbumAnonymous(createdAlbum.DeleteHash, new string[] { "imageId#1","imageId#2" }, "updated album title", "update album description", ImgurAlbumPrivacy.Hidden, ImgurAlbumLayout.Blog, "imageId#2");
``` 
#### Delete
```csharp
 bool deleteAlbum = await imgur.DeleteAlbumAnonymous(album.DeleteHash);
``` 
#### Add images
```csharp
 bool addImagesResult = await imgur.AddImagesToAlbumAnonymous(album.DeleteHash, new string[] { "imageId#1","imageId#2" });
``` 
#### Remove images
```csharp
bool removeImagesResult = await imgur.RemoveImagesFromAlbumAnonymous(album.DeleteHash, new string[] {  "imageId#1" });
``` 
#### Get Album
Will NOT return deleteHash
```csharp
ImgurAlbum album = await imgur.GetAlbum("albumId");
``` 

## License
The MIT License (MIT)

Copyright (c) [2015] [Andreas Lindgren]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
