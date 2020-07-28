using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Titan.UFC.GraphQL.Libraries
{
    public partial class ErrorResponse
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.Errors,Formatting.None);
        }
    }

    public partial class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("locations")]
        public Location[] Locations { get; set; }

        [JsonProperty("extensions")]
        public Extensions Extensions { get; set; }
    }

    public partial class Extensions
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("line")]
        public long Line { get; set; }

        [JsonProperty("column")]
        public long Column { get; set; }
    }

    [Serializable]
    public class GraphQLException:Exception
    {
        private const string _defaultExceptionMessage = "Error while Exceuting Test Case";
        private HttpStatusCode statusCode;
        private ErrorResponse error;

        public GraphQLException(HttpStatusCode _statusCode, ErrorResponse _err):base(_defaultExceptionMessage)
        {
            statusCode = _statusCode;
            error = _err;
        }

        public HttpStatusCode StatusCode
        {
            get { return this.statusCode; }
        }

        public ErrorResponse Error
        {
            get { return this.error; }
        }
    }
}
