using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements.Standard;

public class TextInput : IWrapsElement
{
    public TextInput(IWebElement webElement)
    {
        WrappedElement = webElement;
    }

    public IWebElement WrappedElement { get; private set; }

    public virtual string Text => WrappedElement.Text;
    public virtual bool Enabled => WrappedElement.Enabled;
    public virtual bool Displayed => WrappedElement.Displayed;

    public virtual void SetText(string? text)
    {
        if (text == null) return;

        Clear();
        WrappedElement.SendKeys(text);
    }

    public virtual void Clear()
    {
        WrappedElement.Clear();
    }
}
