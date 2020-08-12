using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Titan.UFC.GraphQL.Libraries
{
    [Serializable]
    public class Utility
    {
        public static HttpContent GetHttpContent(string body)
        {
            var buffer = Encoding.UTF8.GetBytes(body);
            var requestObject = new ByteArrayContent(buffer);
            requestObject.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            requestObject.Headers.ContentLength = buffer.Length;

            return requestObject;
        }
    }
}
