using Microsoft.Playwright;

using PageObjectModelPW.TestCases;

namespace PageObjectModelPW.pages
{
    internal class HomePage : BasePage
    {
        public HomePage(IPage page) : base(page)
        {


        }

        public async Task<NewCarsPage> FindNewCars( )
        {


            var keyWordDriven = new keyWordDriven(page);
            await keyWordDriven.MouseOver("HomePage", "newcars");
            await keyWordDriven.Click("HomePage", "findnewcars");
            await page.Locator("//img[@title='Maruti Suzuki Cars']").ClickAsync();



            return new NewCarsPage(page);
        }

        public async Task SearchCars( )
        {
            await page.Locator("//input[@placeholder='Search']").FillAsync("BMW");
        }

        public void GoToPopularCars( )
        {

        }

        public void GoToUpComingCars( )
        {

        }


    }
}
