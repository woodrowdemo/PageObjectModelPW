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
            fileName = "Extent_" + currentTime.ToString("yyyyMMdd_HHmmss") + ".html";
            extent = CreateInstance(fileName);


        }

        public static ExtentReports CreateInstance(string filename)
        {
            // Build a full path for the report file inside the solution 'reports' folder
            var projectRoot = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var reportsDir = Path.Combine(projectRoot, "reports");
            if (!Directory.Exists(reportsDir))
            {
                Directory.CreateDirectory(reportsDir);
            }

            var reportPath = Path.Combine(reportsDir, filename);

            var htmlReporter = new ExtentSparkReporter(reportPath);
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.DocumentTitle = "Test Artifacts";
            htmlReporter.Config.ReportName = "Test Results";
            htmlReporter.Config.Encoding = "utf-8";

            // Create and configure ExtentReports instance (was missing before)
            var localExtent = new ExtentReports();
            localExtent.AttachReporter(htmlReporter);

            localExtent.AddSystemInfo("Executed by:", "Automation Team");
            localExtent.AddSystemInfo("Organization:", "Woodrows Extent Report Demo");
            localExtent.AddSystemInfo("Build No :", DateTime.Now.ToString("MM-dd-yyyy_HHmmss"));


            return localExtent;


        }

        [OneTimeTearDown]
        public void OneTimeTearDown( )
        {
            extent.Flush();
            log.Info("Test Execution Completed!!!");


        }

        [SetUp]
        public async Task BeforeEachTest( )
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
            await page.SetViewportSizeAsync(1500, 1000);

            // Navigate to configured test site URL (use configuration field)
            var url = configuration?["Appsettings:testsiteurl"];
            if (string.IsNullOrWhiteSpace(url))
            {
                Assert.Fail("Test site URL is not configured (Appsettings:testsiteurl).");
            }

            await page.GotoAsync(url);
            return (browser, page);
        }
    }
}

















