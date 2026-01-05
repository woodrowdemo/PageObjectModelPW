using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    internal class NewCarsPage : BasePage
    {
        public NewCarsPage(IPage page) : base(page)
        {


        }



        public async Task<ToyotaCarsPage> GoToToyota( )
        {
            await keyword.Click("NewCarsPage", "toyota");
            //await page.Locator("//img[@title='Toyota Cars']").ClickAsync();

            return new ToyotaCarsPage(page);
        }

        public async Task<BMWCarsPage> GoToBMW( )
        {
            await keyword.Click("NewCarsPage", "bmwcar");
            //await page.Locator("//div[normalize-space()='BMW']").ClickAsync();
            return new BMWCarsPage(page);
        }
        public async Task<HondasCarsPage> GoToHonda( )
        {
            await keyword.Click("NewCarsPage", "hondacar");
            //await page.Locator("//img[@title='Honda']").ClickAsync();
            return new HondasCarsPage(page);
        }
        public async Task<MGCarsPage> GoToMG( )
        {
            await keyword.Click("NewCarsPage", "mgcar");
            //await page.Locator("//img[@title='Maruti Suzuki']").ClickAsync();
            return new MGCarsPage(page);
        }

    }
}
