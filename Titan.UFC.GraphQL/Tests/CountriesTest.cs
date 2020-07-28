using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Titan.UFC.GraphQL.Libraries;
using Titan.UFC.GraphQL.Model;
using Titan.UFC.GraphQL.Setup;

namespace Tests
{
    public class CountriesTest: BaseTest
    {
        [TestCase(TestName = "Get a specific Country",Category = "Countries Operation,Get One Country", Description = "To get a specific country information")]
        [Category("Smoke")]
        public async Task GetOneCountry()
        {
            var query = @"query($id:String) {
	                        Country(_id:$id) {
                            _id
                            name
                          }
                        }";
            var variables = new { id = "51" };
            var responseObject = await GraphQLClient.QueryAsync(query,variables);
            Assert.IsNull(responseObject.GetException());
            var countryList = responseObject.GetParsedData<CountryList>();
            Assert.AreEqual(countryList.Country.Count, 1);
        }

        [TestCase(TestName = "Get All Countries")]
        public async Task GetAllCountries()
        {
            var query = @"{
	                        Country{
                            _id
                            name
                          }
                        }";
            var responseObject = await GraphQLClient.QueryAsync(query);
            Assert.IsNull(responseObject.GetException());
            var countryList = responseObject.GetParsedData<CountryList>();
            Assert.IsTrue(countryList.Country.Count==250);
        }

        [TestCase(TestName = "Failure TestCase", Description = "This is for report validation")]
        public async Task GetCountryInvalidQuery()
        {
            var query = @"query($id:String){
                          Country{
                            name}
                          Timezone{
                            _id
                            countries(_id:$id)
                          }
                        }";
            var variables = new { id = "51" };
            try
            {
                var responseObject = await GraphQLClient.QueryAsync(query, variables);
                var countryList = responseObject.GetParsedData<CountryList>();
            }
            catch(GraphQLException ex)
            {
                //ReportEvent.Fail(Constants.currentTest, "Bad Response from Server: " + ex.StatusCode);
                ReportHelper.LogFail(currentTest, "Exception Occured: Message: " + ex.Error.ToString());
            }
        }
    }
}