using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Titan.UFC.GraphQL.Setup
{
    public abstract class Constants
    {
        public static string FrameworkPath { get; }
        public static string ReportsPath { get; }
        public static string ReportConfigPath { get; }
        public static string URL { get; }
        public static string TestEnvironment { get; }

        static Constants()
        {
            string assemblyFullPath = new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath).Directory.FullName;
            string reportsFolder = Path.Combine("Results", DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss"));

            FrameworkPath = Uri.UnescapeDataString(new Uri(assemblyFullPath.Remove(assemblyFullPath.IndexOf("bin\\"))).LocalPath);
            ReportsPath = Path.Combine(Constants.FrameworkPath, reportsFolder);
            Directory.CreateDirectory(Constants.ReportsPath);
            ReportsPath = Path.Combine(Constants.ReportsPath, "dashboard.html");
            ReportConfigPath = Path.Combine(Constants.FrameworkPath, "Results", "ReportConfig.xml");

            var builder = new ConfigurationBuilder().AddJsonFile("config.json");
            var configuration = builder.Build();

            URL = configuration["URL"];
            TestEnvironment = configuration["TestEnvironment"];
        }
    }
}
