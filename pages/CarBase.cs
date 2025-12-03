using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    public class CarBase
    {
        IPage page;

        public CarBase(IPage page) => this.page = page;

        public async Task<string> GetCarTitle( )
        {
            return await page.Locator("//header/h1").InnerTextAsync();
        }
    }
}
