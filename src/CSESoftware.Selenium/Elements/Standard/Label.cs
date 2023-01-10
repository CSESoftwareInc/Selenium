using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements.Standard;

public class Label : IWrapsElement
{
    public Label(IWebElement webElement)
    {
        WrappedElement = webElement;
    }

    public IWebElement WrappedElement { get; private set; }
    public virtual string Text => WrappedElement.Text;
    public virtual bool Displayed => WrappedElement.Displayed;
}
