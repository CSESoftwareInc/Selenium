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

    protected virtual void WaitFor(Func<IWebDriver, bool> condition, double seconds = 5, int retries = 1)
    {
        var attempt = 0;

        while (attempt <= retries)
        {
            try
            {
                var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(seconds));
                wait.Until(condition);
                return;
            }
            catch (WebDriverException ex) when (IsRetryableException(ex))
            {
                if (attempt == retries)
                    throw; // out of retries, throw to fail the test

                attempt++;
            }
            catch (WebDriverTimeoutException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    protected virtual bool TryWaitFor(Func<IWebDriver, bool> condition, double seconds = 5, int retries = 1)
    {
        try
        {
            WaitFor(condition, seconds, retries);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static bool IsRetryableException(WebDriverException ex)
    {
        return ex
            is StaleElementReferenceException
            or NoSuchElementException
            or ElementNotInteractableException
            or ElementClickInterceptedException
            or InvalidElementStateException
            or ElementNotVisibleException
            or MoveTargetOutOfBoundsException
            or JavaScriptException;
    }

    protected virtual void WaitForElementExists(By by, double seconds = 5, int retries = 1)
    {
        WaitFor(e => e.ElementExists(by), seconds, retries);
    }

    protected virtual void WaitForElementExists(Expression<Func<T, object>> property, double seconds = 5, int retries = 1)
    {
        WaitForElementExists(GetByForProperty(property), seconds, retries);
    }

    protected virtual void WaitForElementDisplayed(By by, double seconds = 5, int retries = 1)
    {
        WaitFor(e => e.ElementDisplayed(by), seconds, retries);
    }

    protected virtual void WaitForElementDisplayed(Expression<Func<T, object>> property, double seconds = 5, int retries = 1)
    {
        WaitForElementDisplayed(GetByForProperty(property), seconds, retries);
    }

    protected virtual void WaitForElementHidden(By by, double seconds = 5, int retries = 1)
    {
        WaitFor(e => !e.ElementDisplayed(by), seconds, retries);
    }

    protected virtual void WaitForElementHidden(Expression<Func<T, object>> property, double seconds = 5, int retries = 1)
    {
        WaitForElementHidden(GetByForProperty(property), seconds, retries);
    }

    protected virtual By GetByForProperty(Expression<Func<T, object>> property)
    {
        var memberExpression = property.Body switch
        {
            MemberExpression me => me,
            UnaryExpression { Operand: MemberExpression me } => me,
            _ => throw new ElementException("Invalid property expression")
        };

        if (memberExpression.Member is not PropertyInfo prop)
            throw new ElementException("Expression does not refer to a property");

        if (prop == null) throw new ElementException($"Could not find property");

        var attribute = prop.GetCustomAttribute(typeof(FindsByAttribute), false);

        if (attribute == null) throw new ElementException($"FindsBy attribute missing from property");

        var by = ((FindsByAttribute)attribute).Finder;

        return by;
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
