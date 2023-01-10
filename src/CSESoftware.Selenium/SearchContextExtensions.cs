using OpenQA.Selenium;

namespace CSESoftware.Selenium;

public static class SearchContextExtensions
{
    public static bool ElementExists(this ISearchContext driver, By by)
    {
        var elements = driver.FindElements(by);
        return elements.Any();
    }
}
