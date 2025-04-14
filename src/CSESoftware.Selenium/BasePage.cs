using CSESoftware.Selenium.Elements;
using CSESoftware.Selenium.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System.Linq.Expressions;
using System.Reflection;

namespace CSESoftware.Selenium;

public abstract class BasePage<T> where T : BasePage<T>
{
    protected readonly IWebDriver WebDriver;

    public BasePage(IWebDriver driver)
    {
        WebDriver = driver;
        PageFactory.InitElements(WebDriver, this);
        SetWebDriverInChildObjects(WebDriver);
    }

    public static T Load(IWebDriver driver)
    {
        var page = Activator.CreateInstance(typeof(T), driver);

        if (page == null) throw new PageException("Page was unable to load.");

        return (T)page;
    }

    /// <summary>
    /// Verifies you are on the page you expect to be on.
    /// This is not environment specific as it only looks at the path
    /// </summary>
    /// <param name="path"> ex. "/Home/Index.html" </param>
    protected void VerifyUrl(string path)
    {
        var currentUrl = new Uri(WebDriver.Url);
        var currentPath = currentUrl.AbsolutePath;
        var equal = currentPath.Equals(path, StringComparison.OrdinalIgnoreCase);

        if (!equal) throw new PageException($"Path does not match (Expected: {path}, Actual: {currentPath})");
    }

    protected virtual void WaitFor(Func<IWebDriver, bool> condition, double seconds = 5)
    {
        var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(seconds));
        wait.Until(condition);
    }

    protected virtual void WaitForElement(By by, double seconds = 5)
    {
        WaitFor(e => e.ElementExists(by), seconds);
    }

    protected virtual void WaitForElement(Expression<Func<T, object>> property, double seconds = 5)
    {
        var prop = typeof(T).GetProperty(ParsePropertyName(property), BasePage<T>.BindingFlags);
        var attribute = prop?.GetCustomAttribute(typeof(FindsByAttribute), false);

        if (attribute == null) throw new ElementException("FindsBy attribute missing from property");

        WaitForElement(((FindsByAttribute)attribute).Finder, seconds);
    }

    private static string ParsePropertyName(Expression<Func<T, object>> property)
    {
        var body = property.Body.ToString();
        return body.Split('.').Last();
    }

    private static BindingFlags BindingFlags => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    private void SetWebDriverInChildObjects(IWebDriver driver)
    {
        var properties = typeof(T).GetProperties(BasePage<T>.BindingFlags);
        var propertiesNeedingWebDriver = properties
            .Where(prop => typeof(INeedWebDriver).IsAssignableFrom(prop.PropertyType));

        foreach (var property in propertiesNeedingWebDriver)
        {
            var instanceNeedingDriver = property.GetValue(this);
            var driverProperty = property.PropertyType.GetProperty(nameof(INeedWebDriver.WebDriver));

            driverProperty?.SetValue(instanceNeedingDriver, driver);
        }
    }
}
