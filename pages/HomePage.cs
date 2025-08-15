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
            await page.Locator("//*[@id=\"root\"]/div[2]/header/div/nav/ul/li[1]/div[1]").HoverAsync();
            await page.Locator("//div[contains(text(),'Find New Cars')]").ClickAsync();
            //await page.Locator("//img[@title='Maruti Suzuki Cars']").ClickAsync();
            await page.Locator("a[href='/maruti-suzuki-cars/']").ClickAsync();


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
