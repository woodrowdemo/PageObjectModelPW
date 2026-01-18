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
            string locatorValue = null;

            //Load the XML File
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\locators.xml");

            //Get the root element
            XmlElement root = xmlDoc.DocumentElement;

            //Construct XPATH expression to select the specified element under the specified 
            //page with the given locator type
            string xpath = $"/locators/{pageName}/{elementName}";

            //Select the locator value node
            XmlNode locatorValueNode = root.SelectSingleNode(xpath);

            if (locatorValueNode != null)
            {
                locatorValue = locatorValueNode.InnerText;
            }

            return locatorValue;
        }
    }
}
