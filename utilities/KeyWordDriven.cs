using Microsoft.Playwright;

using PageObjectModelPW.testcases;
using PageObjectModelPW.utilities;

namespace PageObjectModelPW.TestCases
{
    public class keyWordDriven
    {
        private readonly IPage _page;

        public keyWordDriven(IPage page1)
        {
            _page = page1;
        }

        public async Task Type(string pageName, string locatorName, string value)
        {
            BaseTest.test.Info("Getting text from the element : " + locatorName + " entered the value as : " + value);
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).FillAsync(value);
        }

        public async Task Click(string pageName, string locatorName)
        {
            BaseTest.test.Info("Clicking on an element : " + locatorName);
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).ClickAsync();
        }

        public async Task MouseOver(string pageName, string locatorName)
        {
            BaseTest.test.Info("Hovering over element : " + locatorName);
            await _page.HoverAsync(XMLLocatorReader.GetLocatorValue(pageName, locatorName));
        }

        internal async Task<string> GetText(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
