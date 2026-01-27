using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System;

namespace PageObjectModelPW.utilities
{
    internal static class RepairAgent
    {
        // Very small proof-of-concept: try to find an element by text or common attributes when a locator fails.
        // Returns a selector string or null. Also logs suggestions.
        public static async Task<string> FindAlternativeAsync(IPage page, string pageName, string elementName)
        {
            var fallbackText = elementName;

            // Try a button or link with the visible text
            try
            {
                var byText = page.Locator($"text=\"{fallbackText}\"");
                if (await byText.CountAsync() > 0 && await byText.IsVisibleAsync())
                {
                    var sel = $"text=\"{fallbackText}\"";
                    await LogSuggestionAsync(pageName, elementName, sel);
                    await PersistSuggestionToXmlAsync(pageName, elementName, sel);
                    return sel;
                }
            }
            catch { }

            // Try contains text
            try
            {
                var contains = page.Locator($"xpath=//*[contains(normalize-space(.), '{EscapeForXPath(fallbackText)}')]");
                if (await contains.CountAsync() > 0 && await contains.IsVisibleAsync())
                {
                    var sel = $"xpath=//*[contains(normalize-space(.), '{EscapeForXPath(fallbackText)}')]";
                    await LogSuggestionAsync(pageName, elementName, sel);
                    await PersistSuggestionToXmlAsync(pageName, elementName, sel);
                    return sel;
                }
            }
            catch { }

            // Try role=button with name
            try
            {
                var byRole = page.GetByRole(AriaRole.Button, new() { Name = fallbackText });
                if (await byRole.CountAsync() > 0 && await byRole.IsVisibleAsync())
                {
                    var sel = $"role=button[name=\"{fallbackText}\"]";
                    await LogSuggestionAsync(pageName, elementName, sel);
                    await PersistSuggestionToXmlAsync(pageName, elementName, sel);
                    return sel;
                }
            }
            catch { }

            return null;
        }

        static string EscapeForXPath(string s)
        {
            if (s.Contains("'"))
            {
                var parts = s.Split('\'');
                return string.Join("', \"'\", '", parts);
            }
            return s;
        }

        static async Task LogSuggestionAsync(string pageName, string elementName, string selector)
        {
            try
            {
                var projectRoot = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                var repairsDir = Path.Combine(projectRoot, "repairs");
                if (!Directory.Exists(repairsDir)) Directory.CreateDirectory(repairsDir);

                var file = Path.Combine(repairsDir, "suggestions.jsonl");
                var entry = new
                {
                    timestamp = DateTime.UtcNow.ToString("o"),
                    page = pageName,
                    element = elementName,
                    selector = selector
                };
                var line = System.Text.Json.JsonSerializer.Serialize(entry);
                await File.AppendAllTextAsync(file, line + Environment.NewLine);
            }
            catch { }
        }

        static async Task PersistSuggestionToXmlAsync(string pageName, string elementName, string selector)
        {
            try
            {
                var xmlPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "resources", "locators.xml");
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                var root = xmlDoc.DocumentElement;
                var xpath = $"/locators/{pageName}/{elementName}";
                var node = root.SelectSingleNode(xpath) as XmlElement;
                if (node == null) return;

                // Append a <suggested> child
                var suggested = xmlDoc.CreateElement("suggested");
                suggested.SetAttribute("timestamp", DateTime.UtcNow.ToString("o"));
                suggested.InnerText = selector;
                node.AppendChild(suggested);

                xmlDoc.Save(xmlPath);
            }
            catch { }
        }
    }
}
