using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CSESoftware.Selenium.Elements.Standard;

public class Dropdown : IWrapsElement
{
    public Dropdown(IWebElement webElement)
    {
        WrappedElement = webElement;
    }

    public IWebElement WrappedElement { get; private set; }
    protected SelectElement SelectElement => new(WrappedElement);

    public virtual List<DropdownOption> Options =>
        SelectElement.Options.Select(Map).ToList();

    public virtual List<DropdownOption> AvailableOptions =>
        Options.Where(x => x.Enabled).ToList();

    public virtual DropdownOption SelectedOption => Map(SelectElement.SelectedOption);

    public virtual void SelectOption(string? text)
    {
        if (text == null) return;

        SelectElement.SelectByText(text);
    }

    protected virtual DropdownOption Map(IWebElement element)
    {
        return new DropdownOption
        {
            Text = element.Text,
            Enabled = IsEnabled(element)
        };
    }

    protected virtual bool IsEnabled(IWebElement element)
    {
        return element.GetAttribute("disabled") == null;
    }
}
