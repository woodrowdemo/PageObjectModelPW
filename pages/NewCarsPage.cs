using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    internal class NewCarsPage : BasePage
    {
        public NewCarsPage(IPage page) : base(page)
        {


        }



        public async Task GoToToyota( )
        {
            await page.Locator("//img[@title='Toyota Cars']").ClickAsync();
        }

        public async Task GoToBMW( )
        {
            await page.Locator("//div[normalize-space()='BMW']").ClickAsync();
        }
        public async Task GoToHonda( )
        {
            await page.Locator("//img[@title='Honda']").ClickAsync();

        }
        public async Task GoToMG( )
        {
            await page.Locator("//img[@title='MG']").ClickAsync();

        }

    }
}
