using Microsoft.Playwright;

using PageObjectModelPW.pages;

namespace PageObjectModelPW.testcases
{

    [TestFixture]
    internal class FindNewCarsTest( )
    {
        [Test]
        public async Task FindCarTest( )
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var page = await browser.NewPageAsync();
            await page.SetViewportSizeAsync(1920, 1080);
            await page.GotoAsync("https://www.carwale.com/");

            HomePage homePage = new HomePage(page);
            await Task.Delay(5000);
            await homePage.FindNewCars();
            await Task.Delay(5000); // Wait for the hover effect to take place
        }

    }
}
