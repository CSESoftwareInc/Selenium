using CSESoftware.Selenium.Exceptions;
using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements;

public class NavigationLink<T> : IWrapsElement, INeedWebDriver
    where T : BasePage<T>
{
    public NavigationLink(IWebElement webElement)
    {
        WrappedElement = webElement;
    }

    public IWebElement WrappedElement { get; private set; }
    public IWebDriver? WebDriver { get; set; }
    public virtual bool Enabled => WrappedElement.Enabled;

    public virtual T Click()
    {
        WrappedElement.Click();

        if (WebDriver == null) throw new MissingWebDriverException("Unable to navigate");

        return BasePage<T>.Load(WebDriver);
    }
}
