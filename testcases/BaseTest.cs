using System.Collections.Concurrent;

using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

using log4net;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using NUnit.Framework.Interfaces;

namespace PageObjectModelPW.testcases
{
    public class BaseTest
    {
        // Playwright, Extent Reports, Logs, Configuration, fix, capture shots etc.

        protected IPlaywright playwright;
        public static ExtentReports extent;
        public static ExtentTest test;

        private static readonly ILog log = LogManager.GetLogger(typeof(BaseTest));

        IConfiguration configuration;
        public static string fileName;

        // Track attempt counts per test so screenshots and logs can include attempt number
        private static readonly ConcurrentDictionary<string, int> _attemptCounts = new();

        [OneTimeSetUp]
        public void OneTimeSetUp( )
        {
            log.Info("Test Execution Started!!!");
            var resourcePath = Path.Combine(AppContext.BaseDirectory, "resources");
            if (!Directory.Exists(resourcePath))
            {
                throw new DirectoryNotFoundException($"The resources directory was not found at path: {resourcePath}");
            }

            configuration = new ConfigurationBuilder()
                .SetBasePath(resourcePath)
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
            // Track and increment attempt count for this test
            var fullName = TestContext.CurrentContext.Test.FullName ?? TestContext.CurrentContext.Test.Name;
            var attempt = _attemptCounts.AddOrUpdate(fullName, 1, (_, v) => v + 1);

            test = extent.CreateTest($"{TestContext.CurrentContext.Test.ClassName} - {TestContext.CurrentContext.Test.Name}");
            test.Info($"Attempt #{attempt}");
            Console.WriteLine($"[TestAttempt] {fullName} attempt #{attempt}");

            playwright = await Playwright.CreateAsync();
        }

        public static async Task CaptureScreenshot(IPage page)
        {
            DateTime currentTime = DateTime.Now;

            // Determine attempt number for current test (if available)
            var fullName = TestContext.CurrentContext.Test.FullName ?? TestContext.CurrentContext.Test.Name;
            _attemptCounts.TryGetValue(fullName, out var attempt);

            // include attempt in file name to differentiate retries
            fileName = currentTime.ToString("yyyy-MM-dd_HHmmss") + $"_attempt{attempt}.jpg";

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

















