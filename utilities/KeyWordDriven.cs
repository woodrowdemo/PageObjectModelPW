using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using PageObjectModelPW.utilities;

namespace PageObjectModelPW.TestCases
{
    internal class KeyWordDriven
    {

        static IBrowser browser;
        static IPage page;



        public static async Task Type(string pageName, string locatorName, string value)
        {
            await page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).FillAsync(value);

        }

        public static async Task Click(string pageName, string locatorName)
        {
            await page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).ClickAsync();

        }

        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // var playwright = await Playwright.CreateAsync();

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
