namespace CSESoftware.Selenium.Elements;

public record DropdownOption
{
    public string Text { get; set; } = "";
    public bool Enabled { get; set; }
}
