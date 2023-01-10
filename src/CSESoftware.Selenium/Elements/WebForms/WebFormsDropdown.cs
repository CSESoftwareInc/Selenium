using CSESoftware.Selenium.Elements.Standard;
using OpenQA.Selenium;

namespace CSESoftware.Selenium.Elements.WebForms;

public class WebFormsDropdown : Dropdown
{
    public WebFormsDropdown(IWebElement webElement) : base(webElement)
    {
    }

    protected override DropdownOption Map(IWebElement element)
    {
        return new DropdownOption
        {
            Text = element.GetValue(),
            Enabled = IsEnabled(element)
        };
    }
}
