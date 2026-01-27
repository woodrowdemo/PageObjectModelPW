using Microsoft.Playwright;

using PageObjectModelPW.testcases;
using PageObjectModelPW.utilities;

/*
 KeyWordDriven — Provides keyword-style wrapper methods around Playwright's IPage:
 - Actions: Type, Click, MouseOver, GetText, IsVisible, SelectDropdownByValue, WaitForElement, UploadFile, Clear
 - Resolves locators via `XMLLocatorReader.GetLocatorValue(pageName, locatorName)`
 - Logs actions to `BaseTest.test` for reporting
*/

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
            BaseTest.test.Info("Typing into element : " + locatorName + " value: " + value);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                // Try RepairAgent with the pageName and locatorName as fallback hint
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.FillAsync(value);
        }

        public async Task Click(string pageName, string locatorName)
        {
            BaseTest.test.Info("Clicking on an element : " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.ClickAsync();
        }

        public async Task MouseOver(string pageName, string locatorName)
        {
            BaseTest.test.Info("Hovering over element : " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.HoverAsync();
        }

        public async Task<string> GetText(string pageName, string locatorName)
        {
            BaseTest.test.Info("Getting text from the element: " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            return await locator.InnerTextAsync();
        }

        public async Task<bool> IsVisible(string pageName, string locatorName)
        {
            BaseTest.test.Info("Checking visibility of element: " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) return false;
            return await locator.IsVisibleAsync();
        }

        public async Task SelectDropdownByValue(string pageName, string locatorName, string value)
        {
            BaseTest.test.Info($"Selecting value '{value}' from dropdown: {locatorName}");
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.SelectOptionAsync(new SelectOptionValue { Value = value });
        }

        public async Task WaitForElement(string pageName, string locatorName, int timeoutMs = 5000)
        {
            BaseTest.test.Info($"Waiting for element: {locatorName}");
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.WaitForAsync(new LocatorWaitForOptions { Timeout = timeoutMs });
        }

        public async Task UploadFile(string pageName, string locatorName, string filePath)
        {
            BaseTest.test.Info($"Uploading file '{filePath}' to element: {locatorName}");
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.SetInputFilesAsync(filePath);
        }

        public async Task Clear(string pageName, string locatorName)
        {
            BaseTest.test.Info("Clearing value of element: " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                var alt = await RepairAgent.FindAlternativeAsync(_page, pageName, locatorName);
                if (!string.IsNullOrEmpty(alt)) locator = _page.Locator(alt);
            }

            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.FillAsync(string.Empty);
        }
    }
}
