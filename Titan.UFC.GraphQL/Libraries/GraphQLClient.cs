using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Titan.UFC.GraphQL.Setup;
using AventStack.ExtentReports;

namespace Titan.UFC.GraphQL.Libraries
{
    public class GraphQLClient
    {
        private ExtentTest test;
        private Uri BaseURL { get; set; }

        public GraphQLClient(ExtentTest test)
        {
            this.test = test;
            BaseURL = new Uri(Constants.URL);
        }

        private class GraphQLQuery
        {
            // public string OperationName { get; set; }
            public string query { get; set; }
            public object variables { get; set; }
        }

        public class GraphQLQueryResult
        {
            private string raw;
            private JObject data;
            private Exception Exception;
            public GraphQLQueryResult(string text, Exception ex = null)
            {
                Exception = ex;
                raw = text;
                data = text != null ? JObject.Parse(text) : null;
                
                if(!(ex is null))
                    throw ex;
            }
            public Exception GetException()
            {
                return Exception;
            }
            public string GetRawData()
            {
                return raw;
            }
            public T GetValue<T>(string key)
            {
                if (data == null) return default(T);
                try
                {
                    return JsonConvert.DeserializeObject<T>(this.data["data"][key].ToString());
                }
                catch
                {
                    return default(T);
                }
            }
            public dynamic GetValue(string key)
            {
                if (data == null) return null;
                try
                {
                    return JsonConvert.DeserializeObject<dynamic>(this.data["data"][key].ToString());
                }
                catch
                {
                    return null;
                }
            }
            public T GetParsedData<T>()
            {
                return JsonConvert.DeserializeObject<T>(this.data["data"].ToString());
            }
        }

        public async Task<GraphQLQueryResult> QueryAsync(string query, object variables=null)
        {
            try
            {
                ReportHelper.LogInfo(test,"URL: " + Constants.URL);
                var fullQuery = new GraphQLQuery()
                {
                    query = query,
                    variables = variables,
                };
                string jsonContent = JsonConvert.SerializeObject(fullQuery);
                ReportHelper.LogInfo(test,"Request Object");
                ReportHelper.LogCodeBlock(test,jsonContent);

                var buffer = Encoding.UTF8.GetBytes(jsonContent);
                var requestObject = new ByteArrayContent(buffer);
                requestObject.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                requestObject.Headers.ContentLength = buffer.Length;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = BaseURL;

                    //httpClient.DefaultRequestHeaders.Add("authorization", "ZGFpc3lAYXBvbGxvZ3JhcGhxbC5jb20=");

                    var response = await httpClient.PostAsync("graphql", requestObject);
                    ReportHelper.LogHighlight(test,$"Response Code: {(int)response.StatusCode} - {response.StatusCode}");
                    if (response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        ReportHelper.LogCodeBlock(test, await response.Content.ReadAsStringAsync());
                        return new GraphQLQueryResult(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        ReportHelper.LogCodeBlock(test, await response.Content.ReadAsStringAsync());
                        var responseResult = await response.Content.ReadAsStringAsync();
                        var ErrorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseResult);
                        throw new GraphQLException(response.StatusCode, ErrorResponse);
                    }
                }                    
            }
            catch (ArgumentNullException ex)
            {
                ReportHelper.LogWarning(test, "Exception Occurred: " + ex.Message);
                return new GraphQLQueryResult(null, ex);
            }
            catch (HttpRequestException ex)
            {
                ReportHelper.LogWarning(test, "Exception Occurred: " + ex.Message);
                return new GraphQLQueryResult(null, ex);
            }
        }
    }
}