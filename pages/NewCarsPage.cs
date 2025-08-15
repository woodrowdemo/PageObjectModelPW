using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    internal class NewCarsPage
    {

        IPage page;

        public NewCarsPage(IPage page)
        {
            this.page = page;
        }

    }
}
