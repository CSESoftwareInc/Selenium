using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements;

public interface INeedWebDriver
{
    IWebDriver? WebDriver { get; set; }
}
