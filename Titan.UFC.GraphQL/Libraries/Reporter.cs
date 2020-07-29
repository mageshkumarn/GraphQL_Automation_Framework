using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using Titan.UFC.GraphQL.Setup;

namespace Titan.UFC.GraphQL.Libraries
{
    [Serializable]
    public abstract class ReportHelper
    {
        const string _DEFAULT_DOCUMENT_TITLE = "Automation Report";
        const string _DEFAULT_REPORT_NAME = "GraphQL Automation Run";
        const string _DEFAULT_ENCODING = "UTF-8";

        private static ExtentHtmlReporter extentHtmlReporter;
        public static AventStack.ExtentReports.ExtentReports ExtentReports { get; private set; }

        static ReportHelper()
        {
            extentHtmlReporter = new ExtentHtmlReporter(Constants.ReportsPath);
            ExtentReports = new AventStack.ExtentReports.ExtentReports();
            ExtentReports.AttachReporter(extentHtmlReporter);
            if (File.Exists(Constants.ReportConfigPath))
                extentHtmlReporter.LoadConfig(Constants.ReportConfigPath);
            else
            {
                extentHtmlReporter.Config.DocumentTitle = _DEFAULT_DOCUMENT_TITLE;
                extentHtmlReporter.Config.ReportName = _DEFAULT_REPORT_NAME;
                extentHtmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
                extentHtmlReporter.Config.Encoding = _DEFAULT_ENCODING;
            }
        }

        public static void LogInfo(ExtentTest test, string message)
        {
            test.Info(message);
        }

        public static void LogDescription(ExtentTest test, string message)
        {
            test.Info(MarkupHelper.CreateLabel(message, ExtentColor.Grey, ExtentColor.Brown));
        }

        public static void LogHighlight(ExtentTest test, string message)
        {
            test.Info(MarkupHelper.CreateLabel(message, ExtentColor.Yellow ,ExtentColor.Indigo));
        }

        public static void LogCodeBlock(ExtentTest test, string message, CodeLanguage language=CodeLanguage.Json)
        {
            test.Log(Status.Debug,MarkupHelper.CreateCodeBlock(message,language));
        }

        public static void LogWarning(ExtentTest test, string message)
        {
            test.Warning(MarkupHelper.CreateLabel(message, ExtentColor.Orange));
            if(!(TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Warning)
                || TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed)))
            Assert.Warn(message);
        }

        public static void LogPass(ExtentTest test, string message)
        {
            test.Pass(MarkupHelper.CreateLabel(message,ExtentColor.Green));
        }

        public static void LogFail(ExtentTest test, string message)
        {
            test.Fail(MarkupHelper.CreateLabel(message, ExtentColor.Red));
            if (!(TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Warning)
                || TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed)))
                Assert.Fail(message);
        }
    }
}
