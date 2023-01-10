using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements.Standard;

public class Hyperlink : Button
{
    public Hyperlink(IWebElement webElement) : base(webElement)
    {
    }

    public virtual string LinkUrl => WrappedElement.GetAttribute("href");
}
