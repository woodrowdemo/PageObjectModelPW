using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

using log4net;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace PageObjectModelPW.testcases
{
    public class BaseTest
    {
        // Playwright, Extent Reports, Logs, Configuration, fix, capture shots etc.

        protected IPlaywright playwright;
        private static ExtentReports extent;
        public static ExtentTest test;

        private static readonly ILog log = LogManager.GetLogger(typeof(BaseTest));

        IConfiguration configuration;
        private static string fileName;


        [OneTimeSetUp]
        public void OneTimeSetUp( )
        {
            log.Info("Test Execution Started!!!");
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            DateTime currentTime = DateTime.Now;
            string fileName = "Extent_" + currentTime.ToString("yyyyMMdd_HHmmss") + ".html";
            extent = CreateInstance(fileName);


        }

        public static ExtentReports CreateInstance(string filename)
        {


            var htmlReporter = new ExtentSparkReporter(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName);
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.DocumentTitle = "Test Artifacts";
            htmlReporter.Config.ReportName = "Test Results";
            htmlReporter.Config.Encoding = "utf-8";

            extent.AddSystemInfo("Executed by:", "Woodrow Winters");
            extent.AddSystemInfo("Organization:", "Woodrows Extent Report Demo");
            extent.AddSystemInfo("Build No :", "W2A-1234");


            return extent;


        }

        [OneTimeTearDown]
        public void OneTimeTearDown( )
        {
            extent.Flush();
            log.Info("Test Execution Completed!!!");


        }

        [SetUp]
        public async void BeforeEachTest( )
        {

            test = extent.CreateTest($"{TestContext.CurrentContext.Test.ClassName} - {TestContext.CurrentContext.Test.Name}");
            playwright = await Playwright.CreateAsync();
        }

        public static void CaptureScreenshot(IPage page)
        {
            DateTime currentTime = DateTime.Now;
            fileName = currentTime.ToString("yyyy-MM-dd_HHmmss") + ".jpg";
        }

        [TearDown]
        public void AfterEachTest( )
        {

        }
    }
}
