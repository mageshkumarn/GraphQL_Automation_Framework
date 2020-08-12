using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Titan.UFC.GraphQL.Libraries;
using Titan.UFC.GraphQL.Model;

namespace Titan.UFC.GraphQL.Setup
{
    public abstract class RequestHelper
    {
        public static async Task AuthorizeUserAsync()
        {
            var loginQuery = new GraphQLQuery()
            {
                query = @"mutation LoginUser {
  login(email: """ + Constants.LoginEmail + @""")
}",
                variables = null,
            };
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Constants.URL);
                var response = await httpClient.PostAsync("graphql", Utility.GetHttpContent(JsonConvert.SerializeObject(loginQuery)));
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var rawResponse = await response.Content.ReadAsStringAsync();
                    var authToken = JsonConvert.DeserializeObject<LoginResponse>(rawResponse);
                    Constants.TestParameters.Authorization = authToken.data.login;
                }
                else
                {
                    Constants.TestParameters.Authorization = "TOKEN_NOT_GENERATED";
                }
            }
        }
    }
}
