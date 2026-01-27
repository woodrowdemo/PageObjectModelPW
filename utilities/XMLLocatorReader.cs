/* 
 XMLLocatorReader — Reads locator values from `resources\locators.xml`.
 - GetLocatorValue(pageName, elementName) loads the XML and returns the inner text
   of `/locators/{pageName}/{elementName}` or null if not found.
*/
using System.Xml;

namespace PageObjectModelPW.utilities
{
    internal class XMLLocatorReader
    {
        public static string GetLocatorValue(string pageName, string elementName)
        {
            // Return the first available locator value (keeps backward compatibility)
            var values = GetLocatorValues(pageName, elementName);
            return values?.FirstOrDefault();
        }

        public static List<string> GetLocatorValues(string pageName, string elementName)
        {
            var result = new List<string>();

            // Load the XML File
            var xmlPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "resources", "locators.xml");
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            // Get the root element
            XmlElement root = xmlDoc.DocumentElement;

            // Select the element node for the page/element
            string xpath = $"/locators/{pageName}/{elementName}";
            XmlNode elementNode = root.SelectSingleNode(xpath);
            if (elementNode == null) return result;

            // If there are child <locator> nodes, use each one as a candidate
            var locatorChildren = elementNode.SelectNodes("locator");
            if (locatorChildren != null && locatorChildren.Count > 0)
            {
                foreach (XmlNode node in locatorChildren)
                {
                    var txt = node.InnerText?.Trim();
                    if (!string.IsNullOrEmpty(txt)) result.Add(txt);
                }
                return result;
            }

            // Otherwise, treat the element's inner text as either a single locator
            // or a list separated by '||' or newlines
            var inner = elementNode.InnerText?.Trim();
            if (string.IsNullOrEmpty(inner)) return result;

            if (inner.Contains("||"))
            {
                result.AddRange(inner.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));
            }
            else if (inner.Contains("\n"))
            {
                result.AddRange(inner.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));
            }
            else
            {
                result.Add(inner);
            }

            return result;
        }
    }
}
