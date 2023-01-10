using OpenQA.Selenium;

namespace CSESoftware.Selenium;

public static class WebElementExtensions
{
    public static string GetValue(this IWebElement element)
    {
        return element.GetAttribute("value");
    }
}
