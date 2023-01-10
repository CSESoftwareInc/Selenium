using CSESoftware.Selenium.Elements.Standard;
using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements.WebForms;

public class WebFormsTextInput : TextInput
{
    public WebFormsTextInput(IWebElement webElement) : base(webElement)
    {
    }

    public override string Text => WrappedElement.GetValue();
}
