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
        [Retry(2)]
        [Test, TestCaseSource(nameof(GetTestData)), Category("MyTest")]
        public async Task FindCarTest(string carbrand, string browserType, string runmode, string carTitle)
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
            {

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
                        Console.WriteLine("Car title is : " + await BasePage.carBase.GetCarTitle());

                        Assert.That(carTitle.Equals(await BasePage.carBase.GetCarTitle()), "Car titles not matching " + carTitle);
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


                await Task.Delay(2000);
            }
        }
        public static IEnumerable<TestCaseData> GetTestData( )
        {
            var columns = new List<string> { "carbrand", "browserType", "runmode", "carTitle" };

            var path = Path.Combine(AppContext.BaseDirectory, "resources", "TestSpreadSheet.xlsx");
            if (!File.Exists(path)) throw new FileNotFoundException("Spreadsheet not found at: " + path);

            return DataUtil.GetTestDataFromExcel(path, "FindCarTest", columns);
        }

    }
}
