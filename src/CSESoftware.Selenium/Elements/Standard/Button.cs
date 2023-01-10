using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements.Standard;

public class Button : IWrapsElement
{
    public Button(IWebElement webElement)
    {
        WrappedElement = webElement;
    }

    public IWebElement WrappedElement { get; private set; }
    public virtual bool Enabled => WrappedElement.Enabled;
    public virtual bool Displayed => WrappedElement.Displayed;
    public virtual string Text => WrappedElement.Text;

    public virtual void Click()
    {
        WrappedElement.Click();
    }
}
