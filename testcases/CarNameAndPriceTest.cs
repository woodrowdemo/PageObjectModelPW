using Microsoft.Playwright;
using System.IO;
using System;

using PageObjectModelPW.pages;
using PageObjectModelPW.utilities;

namespace PageObjectModelPW.testcases
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    internal class CarNameAndPriceTest : BaseTest
    {
        [Parallelizable(ParallelScope.Self)]
        [Retry(2)]
        [Test, TestCaseSource(nameof(GetTestData)), Category("SmokeTest")]
        public async Task CarNameAndPrice(string carbrand, string browserType, string runmode)
        {

            if (runmode.Equals("N"))
            {

                Assert.Ignore("Ignoring the test as the run mode is No.");
            }

            // Each test gets a new playwright instance
            using var playwrightInstance = await Playwright.CreateAsync();

            var (browser, page) = await CreateBrowserAndPage(playwrightInstance, browserType, new BrowserTypeLaunchOptions { Headless = false });

            HomePage homePage = new HomePage(page);
            NewCarsPage newCar = await homePage.FindNewCars();

            var carBrandsActions = new Dictionary<string, Func<Task>>
                {
                    { "bmw", newCar.GoToBMW },
                    { "honda", newCar.GoToHonda },
                    { "toyota", newCar.GoToToyota },
                    { "mg", newCar.GoToMG },

                };
            try
            {


                if (carBrandsActions.TryGetValue(carbrand.ToLower(), out var navigateToCar))
                {

                    await navigateToCar();
                    await BasePage.carBase.GetCarNameAndPrices();
                }
                else
                {
                    Assert.Fail($"Car brand '{carbrand}' does not exist");

                }

                await Task.Delay(2000);
            }

            catch (Exception)
            {
                await CaptureScreenshot(page);

            }
            finally
            {
                await page.CloseAsync();
                await browser.CloseAsync();
            }
        }
        public static IEnumerable<TestCaseData> GetTestData( )
        {
            var columns = new List<string> { "carbrand", "browserType", "runmode" };

            // Prefer resources copied to output (AppContext.BaseDirectory), fallback to project-root path
            var spreadsheetPath = Path.Combine(AppContext.BaseDirectory, "resources", "TestSpreadSheet.xlsx");
            if (!File.Exists(spreadsheetPath))
            {
                spreadsheetPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "resources", "TestSpreadSheet.xlsx");
            }

            if (!File.Exists(spreadsheetPath))
            {
                throw new FileNotFoundException($"Test spreadsheet not found. Checked path: {spreadsheetPath}");
            }

            return DataUtil.GetTestDataFromExcel(spreadsheetPath, "CarNameAndPrice", columns);
        }

    }
}
