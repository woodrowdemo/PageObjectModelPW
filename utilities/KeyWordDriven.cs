using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using PageObjectModelPW.testcases;
using PageObjectModelPW.utilities;

namespace PageObjectModelPW.TestCases
{
    internal class KeyWordDriven
    {

        static IBrowser browser;
        private static IPage page;

        //public KeyWordDriven(IPage pageInstance) { page = pageInstance; }

        public static async Task Type(string pageName, string locatorName, string value)
        {
            BaseTest.test.Info("Getting text from the element : " + locatorName + "entered the value as : " + value);
            await page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).FillAsync(value);
        }

        public static async Task Click(string pageName, string locatorName)
        {
            BaseTest.test.Info("Click on an element : " + locatorName);
            await page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).ClickAsync();
        }

        public static async Task MouseOver(string pageName, string locatorName)
        {
            BaseTest.test.Info("Click on an element : " + locatorName);
            await page.HoverAsync(XMLLocatorReader.GetLocatorValue(pageName, locatorName));
        }

        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var testSiteUrl = configuration["Appsettings:testsiteurl"];
            if (string.IsNullOrEmpty(testSiteUrl))
            {
                throw new ArgumentNullException(nameof(testSiteUrl), "The test site URL cannot be null or empty.");
            }

            var playwright = await Playwright.CreateAsync();

            if (configuration["Appsettings:browser"].Equals("chrome"))
            {
                browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                });
            }
            else if (configuration["Appsettings:browser"].Equals("firefox"))
            {
                browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                });
            }

            page = await browser.NewPageAsync();
            await page.GotoAsync(configuration["Appsettings:testsiteurl"]);

            await Type("LoginPage", "username", "testuser");
            await Type("LoginPage", "password", "testpassword");
            await Click("LoginPage", "loginbutton");

            await Task.Delay(5000); // Wait for 2 seconds to see the result
        }
    }
}
