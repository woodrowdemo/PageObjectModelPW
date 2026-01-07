using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    public class CarBase
    {
        IPage page;

        //car name - //div/div/div/div/a/h3
        //car price -//div/div/div[3]/div/span/span[1]

        public CarBase(IPage page) => this.page = page;

        public async Task<string> GetCarTitle( )
        {
            return await BasePage.keyword.GetText("CarBase", "cartitle");
            //return await page.Locator("//header/h1").InnerTextAsync();
        }

        public async Task GetCarNameAndPrices( )
        {
            for (int i = 0; i < await page.Locator("//div/div/div[3]/div/span/span[1]").CountAsync(); i++)
            {
                Console.WriteLine(await page.Locator("//div/div/div/div/a/h3").Nth(i).InnerTextAsync() + "------price is : " + await page.Locator("//div/div/div[3]/div/span/span[1]").Nth(i).InnerTextAsync());
            }
        }
    }
}
