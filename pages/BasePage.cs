using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    public class BasePage
    {
        public IPage page;

        public BasePage(IPage page)
        {


            this.page = page;
        }
    }
}
