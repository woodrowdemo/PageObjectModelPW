using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    internal class HomePage : BasePage
    {
        public HomePage(IPage page) : base(page)
        {


        }

        public async Task<NewCarsPage> FindNewCars( )
        {
            await page.Locator("//*[@id=\"root\"]/div[1]/header[1]/div[1]/nav[1]/ul[1]/li[1]/div[1]").HoverAsync();
            await page.Locator("//div[contains(text(),'Find New Cars')]").ClickAsync();
            await page.Locator("//img[@title='Maruti Suzuki Cars']").ClickAsync();
            //await page.Locator("a[href='/maruti-suzuki-cars/']").ClickAsync();

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
