using Microsoft.Playwright;

using PageObjectModelPW.pages;
using PageObjectModelPW.utilities;

namespace PageObjectModelPW.testcases
{

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    internal class FindNewCarsTest : BaseTest
    {

        [Parallelizable(ParallelScope.Self)]
        [Test, TestCaseSource(nameof(GetTestData))]
        public async Task FindCarTest(string carbrand, string browserType, string runmode)
        {

            if (runmode.Equals("N"))
            {

                Assert.Ignore("Ignoring the test as the run mode is No.");
            }

            // Each test gets a new playwright instance
            using var playwrightInstance = await Playwright.CreateAsync();

            var (browser, page) = await CreateBrowserAndPage(playwrightInstance, browserType, new BrowserTypeLaunchOptions { Headless = false });
            //await page.GotoAsync("https://www.carwale.com/");

            HomePage homePage = new HomePage(page);
            NewCarsPage newCar = await homePage.FindNewCars();
            await homePage.FindNewCars();

            try
            {
                if (carbrand.Equals("bmw"))
                {
                    await newCar.GoToBMW();
                }
            }
            catch (Exception ex)
            {
                await CaptureScreenshot(page);

            }
            finally
            {
                await page.CloseAsync();
                await browser.CloseAsync();
            }


            await Task.Delay(2000);
            //await newCar.GoToBMW();
            await Task.Delay(2000);
        }
        public static IEnumerable<TestCaseData> GetTestData( )
        {
            var columns = new List<string> { "carbrand", "browserType", "runmode" };

            return DataUtil.GetTestDataFromExcel(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\TestSpreadSheet.xlsx", "FindCarTest", columns);
        }

    }
}
