using Microsoft.Playwright;

namespace PageObjectModelPW.utilities
{
    internal static class LocatorResolver
    {
        // Try a series of candidate selector strings and return the first ILocator that is visible
        public static async Task<ILocator> ResolveAsync(IPage page, string pageName, string elementName)
        {
            var candidates = XMLLocatorReader.GetLocatorValues(pageName, elementName);
            if (candidates == null || candidates.Count == 0)
                return null;

            foreach (var candidate in candidates)
            {
                try
                {
                    var locator = page.Locator(candidate);
                    var visible = await locator.IsVisibleAsync();
                    if (visible) return locator;
                }
                catch
                {
                    // ignore and try next
                }
            }

            return null;
        }
    }
}
