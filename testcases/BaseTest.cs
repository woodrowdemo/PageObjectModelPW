using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

using log4net;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using NUnit.Framework.Interfaces;
// I stopped on modul 111

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
            extent.AddSystemInfo("Build No :", DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));


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

        public static async Task CaptureScreenshot(IPage page)
        {
            DateTime currentTime = DateTime.Now;
            fileName = currentTime.ToString("yyyy-MM-dd_HHmmss") + ".jpg";

            await page.ScreenshotAsync(new PageScreenshotOptions { Path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "\\screenshots\\" + fileName });
        }





        [TearDown]
        public void AfterEachTest( )
        {

            // Get the test result
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            string message = TestContext.CurrentContext.Result.Message;

            switch (testStatus)
            {
                case TestStatus.Passed:
                    test.Pass("Test Passed");
                    IMarkup markup = MarkupHelper.CreateLabel("PASS", ExtentColor.Green);
                    break;
                case TestStatus.Skipped:
                    test.Skip($"Test Skipped: {message}");
                    markup = MarkupHelper.CreateLabel("SKIPPED", ExtentColor.Yellow);
                    break;
                case TestStatus.Failed:
                    test.Fail($"Test Failed: {message}");
                    test.Fail("<b><font color='red'>Screenshot of failure</font></b><br>", MediaEntityBuilder.CreateScreenCaptureFromPath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "\\screenshots\\" + fileName).Build());
                    markup = MarkupHelper.CreateLabel("FAIL", ExtentColor.Red);
                    break;


            }
            playwright.Dispose();
        }

        protected async Task<(IBrowser, IPage)> CreateBrowserAndPage(IPlaywright playwrightInstance, string browserType, BrowserTypeLaunchOptions launchOptions = null)
        {
            IBrowser browser;
            if (browserType.Equals("chrome", StringComparison.OrdinalIgnoreCase))
            {

                browser = await playwrightInstance.Chromium.LaunchAsync(launchOptions);
            }
            else if (browserType.Equals("firefox", StringComparison.OrdinalIgnoreCase))
            {
                browser = await playwrightInstance.Firefox.LaunchAsync(launchOptions);
            }
            else
            {
                Assert.Fail("Unsupported browser type: " + browserType);
                return (null, null); // This line will never be reached due to Assert.Fail
            }

            IPage page = await browser.NewPageAsync();

            await page.GotoAsync("AppSettings:testsiteurl");
            return (browser, page);
        }
    }
}









