using System;
using System.IO;
using System.Reflection;

namespace ImgurSharp.Test
{
    public class ResourceRetriever
    {
        public static string GetJsonString(string path)
        {
            using (Stream resource = Get(path))
            {
                using (var reader = new StreamReader(resource))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static Stream GetStream(string path)
        {
            return Get(path);
        }

        private static Stream Get(string path)
        {
            Assembly assembly = typeof(ResourceRetriever).GetTypeInfo().Assembly;

            path = string.Format("{0}.{1}", assembly.GetName().Name, path.Replace("/", "."));

            Stream stream = assembly.GetManifestResourceStream(path);

            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream with path: " + path);
            }

            return stream;
        }
    }
}
