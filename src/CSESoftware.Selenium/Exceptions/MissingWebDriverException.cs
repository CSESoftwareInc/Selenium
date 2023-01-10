namespace CSESoftware.Selenium.Exceptions;

public class MissingWebDriverException : Exception
{
    public MissingWebDriverException(string message) : base($"{message}: Missing WebDriver. Verify your page is inheriting from {typeof(BasePage<>)}.")
    {
    }
}
