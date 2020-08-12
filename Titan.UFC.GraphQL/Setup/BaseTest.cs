using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Threading.Tasks;
using Titan.UFC.GraphQL.Libraries;
using Titan.UFC.GraphQL.Setup;

namespace Titan.UFC.GraphQL.Tests
{
    [TestFixture]
    public class BaseTest
    {
        public GraphQLClient GraphQLClient;
        public ExtentTest currentTest;

        [OneTimeSetUp]
        public async Task InitializeSuit()
        {
            ReportHelper.ExtentReports.AddSystemInfo("Test Environment", Constants.TestEnvironment);
            ReportHelper.ExtentReports.AddSystemInfo("Author", "Magesh Kumar");
            ReportHelper.ExtentReports.AddSystemInfo("Contact", "MageshKumar.N@myOrg.com");
            await RequestHelper.AuthorizeUserAsync();
        }

        [SetUp]
        public void InitializeTestAsync()
        {
            currentTest = ReportHelper.ExtentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            var className = TestContext.CurrentContext.Test.FullName.Split(".")[TestContext.CurrentContext.Test.FullName.Split(".").Length - 2];
            //var propertyBag = TestContext.CurrentContext.Test?.Properties["Category"];
            //var tagName = "";
            //foreach (var keys in propertyBag)
            //{
            //    tagName = keys.ToString();
            //}

            currentTest.AssignCategory(className);
            var propertyBag = TestContext.CurrentContext.Test?.Properties["Description"];
            string TestDescription = "";
            foreach (var value in propertyBag)
            {
                TestDescription = value.ToString();
            }
            if (!String.IsNullOrEmpty(TestDescription))
            {
                ReportHelper.LogDescription(currentTest, "Description: " + TestDescription);
            }
            GraphQLClient = new GraphQLClient(currentTest);
        }

        [TearDown]
        public void WrapupTest()
        {
            var finalTestStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var finalTestReultMessage = TestContext.CurrentContext.Result.Message;
            switch (finalTestStatus)
            {
                case TestStatus.Failed:
                    ReportHelper.LogFail(currentTest, "Test Failed with " + finalTestReultMessage);
                    break;
                case TestStatus.Skipped:
                    ReportHelper.LogWarning(currentTest, "Test Skipped with " + finalTestReultMessage);
                    break;
                case TestStatus.Warning:
                    ReportHelper.LogWarning(currentTest, "Test has some errors: " + finalTestReultMessage);
                    break;
                default:
                    ReportHelper.LogPass(currentTest, "Test completed with Pass");
                    break;
            }
        }

        [OneTimeTearDown]
        public void WrapupSuit()
        {
            ReportHelper.ExtentReports.Flush();
        }
    }
}
