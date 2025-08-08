using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    internal class HomePage
    {

        IPage page;
        public async void FindNewCars( )
        {
            await page.Locator("//div[normalize-space()='New Cars']").HoverAsync();
            await page.Locator("//div[normalize-space()='Find New Cars']").ClickAsync();
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
