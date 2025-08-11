using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    internal class HomePage
    {

        IPage page;

        public HomePage(IPage page)
        {
            this.page = page;
        }
        public async Task FindNewCars( )
        {
            await page.Locator("//*[@id=\"root\"]/div[1]/header/div/nav/ul/li[1]/div").HoverAsync();
            await page.Locator("//div[contains(text(),'Find New Cars')]").ClickAsync();
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
