using Microsoft.Playwright;

using PageObjectModelPW.pages;
using PageObjectModelPW.utilities;

namespace PageObjectModelPW.testcases
{

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    internal class FindNewCarsTest( )
    {

        [Parallelizable(ParallelScope.Self)]
        [Test, TestCaseSource(nameof(GetTestData))]
        public async Task FindCarTest(string carbrand, string browserType, string runmode)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var page = await browser.NewPageAsync();
            await page.SetViewportSizeAsync(1500, 1000);
            await page.GotoAsync("https://www.carwale.com/");

            HomePage homePage = new HomePage(page);
            NewCarsPage newCar = await homePage.FindNewCars();
            await homePage.FindNewCars();

            await Task.Delay(2000);
            await newCar.GoToBMW();
            await Task.Delay(2000);
        }
        public static IEnumerable<TestCaseData> GetTestData( )
        {
            var columns = new List<string> { "carbrand", "browserType", "runmode" };

            return DataUtil.GetTestDataFromExcel(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\TestSpreadSheet.xlsx", "FindCarTest", columns);
        }

    }
}
