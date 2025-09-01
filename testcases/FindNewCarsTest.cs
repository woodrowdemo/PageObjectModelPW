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
            await page.SetViewportSizeAsync(1500, 1000);
            await page.GotoAsync("https://www.carwale.com/");

            HomePage homePage = new HomePage(page);
            //await Task.Delay(2000);
            await homePage.FindNewCars();

            NewCarsPage newCar = new NewCarsPage(page);
            await Task.Delay(2000);
            await newCar.GoToBMW();
            await Task.Delay(2000);
        }

    }
}
