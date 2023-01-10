using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements.Standard;

public class Checkbox : IWrapsElement
{
    public Checkbox(IWebElement webElement)
    {
        WrappedElement = webElement;
    }

    public IWebElement WrappedElement { get; private set; }
    public virtual bool Selected => WrappedElement.Selected;
    public virtual bool Enabled => WrappedElement.Enabled;
    public virtual bool Displayed => WrappedElement.Displayed;

    public virtual void Select(bool value)
    {
        if (Selected == value)
            return;

        WrappedElement.Click();
    }
}
