using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurSharp
{
    public class ResponseException : Exception
    {
        public ResponseException(string message) : base(message) {}
    }
}
