# CSESoftware.Selenium

A library to be combined with [DotNetSeleniumExtras.PageObjects.Core](https://github.com/Dreamescaper/DotNetSeleniumExtras.Core) as a way to give you more tools built in when you start your testing with the Page Object Model design pattern.

---

## How to use the BasePage
The BasePage gives you a some handy built in functionality. 

Here is an example page inheriting from BasePage. You pass your page in as a generic for some handy Wait functionality. 

```csharp
public class ProjectPage : BasePage<ProjectPage>
{
	[FindsBy(How.ClassName, "new-project-button")]
	private Button NewProjectButton { get; set; }

	public ProjectPage(IWebDriver webDriver) : base(webDriver)
	{
		WaitForElement(x => x.NewProjectButton, 15);
		VerifyUrl("/Projects");
	}
}
```

#### Constructor
The constructor will call PageFactor.InitElements to gather all of your elements with FindsBy attributes. 

#### Load
Use the static Load function by just calling `ProjectPage.Load(WebDriver);`. This will new up a ProjectPage and have it ready for you to continue on with your testing. 



## Waiting
Waiting is very important with tests. Your UI needs to be in the state you are expecting for your code to execute correctly. Here are a couple ways you can wait using CSESoftware.Selenium

### For elements
On any page inheriting from BasePage, you can call `WaitForElement()` and pass in an expression pointing at the element you want to wait for. Alternatively, you can pass in a By locator.

Optionally, you can pass in the maximum number of seconds it should wait for the element to appear.

```csharp
WaitForElement(x => x.NewProjectButton);
WaitForElement(By.ClassName("new-project-button"), 15);
```

### For conditions
Sometimes, you may want to wait for another condition besides an element appearing. You can do so with `WaitFor()`

```csharp
WaitFor(x => x.Url.Contains("Home"));
```

### For specific lengths of time
Sometimes, the above waiting functions aren't enough, and you need to just pause for a bit. These waits should be used sparingly as it will increase execution time by a set amount where `WaitFor()` and `WaitForElement()` will continue on as soon as the condition is true.

```csharp
Wait.Seconds(3);
Wait.Seconds(1.5);
Wait.Milliseconds(500);
```



## How to create and use elements
The IWebElement is powerful, but it is easy to run into areas where you may want to either expand on its capabilities or even restrict the available options. That is where `IWrapsElement` comes in. 

### IWrapsElement

Here is a simple checkbox element:
```csharp
public class Checkbox : IWrapsElement
{
	public Checkbox(IWebElement webElement)
	{
		WrappedElement = webElement;
	}

	public IWebElement WrappedElement { get; private set; }
	public bool Selected => WrappedElement.Selected;

	public void Select(bool value)
	{
		if (Selected == value)
			return;

		WrappedElement.Click();
	}
}
```

How you use it on a page:
```csharp
// Rather than using IWebElement
[FindsBy(How.Id, "my-checkbox")]
private IWebElement MyCheckbox { get; set; }

// You can use the Checkbox class
[FindsBy(How.Id, "my-checkbox")]
private Checkbox MyCheckbox { get; set; }
```
It is the same syntax you are used to with DotNetSeleniumExtras, but now you you can call `MyCheckbox.Select(true)` to make sure the checkbox is checked without needing to worry about its current state. 

Check out some other elements in [src/CSESoftware.Selenium/Elements](https://github.com/CSESoftwareInc/Selenium/tree/main/src/CSESoftware.Selenium/Elements)


### INeedWebDriver
Occasionally, you will want to encapsulate some logic into an Element, but you need access to the `IWebDriver` to get the functionality you need. This could be for finding elements that appear to be grouped visually but are actually in very different parts of the DOM or it could be for other `IWebDriver` functionality. 

For this type of Element, just inherit from `INeedWebDriver` and the WebDriver will be populated via the constructor in `BasePage`. 


---

CSE Software Inc. is a privately held company founded in 1990. CSE develops software, AR/VR, simulation, mobile, and web technology solutions. The company also offers live, 24x7, global help desk services in 110 languages. All CSE teams are U.S. based with experience in multiple industries, including government, military, healthcare, construction, agriculture, mining, and more. CSE Software is a certified women-owned small business. Visit us online at [csesoftware.com](https://www.csesoftware.com).
