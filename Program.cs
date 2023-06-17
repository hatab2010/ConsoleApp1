using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

var tokens = new List<string>
{
    "682066f9862be02baa0b2d03bfe398aa1764da94",
    "05c11bbd48e196dc0212f5ebb0d06843bc9e962b",
    "ba0676a2d3e7b6644f16c416ee2ad87e6e8c8c30",
    "7eff45addb2c045ed4a2f2ba173b032afdcdd2b0",
    "382b2e3cd519610309b7efd3b508aaeabd1b56c9",
    "6530ac171a2a986dbf2540f989a39916374978d1",
    "3d369c8741fe90ad6008a52788fae2621154bae5",
    "df71e30670784b1aadca3a092f959f4555e150b6",
    "261a518f6ce4e2752a1da020cf3bb05d3d7e6a22",
    "e8676f4ae5fabb5270e3567a7eb93672c7a4acbe"
};

var phantomDriver = new PhantomChromeDriver();
phantomDriver.Navigate().GoToUrl("https://sbermarket.ru/");

foreach (var token in tokens)
{
    Cookie cookie = new(name: "auth_token",
                         value: token,
                         domain: "twitter.com",
                         path: "/",
                         expiry: DateTime.Today.AddDays(1),
                         secure: true,
                         isHttpOnly: false,
                         sameSite: "None");

    phantomDriver.Navigate().GoToUrl("https://twitter.com/");
    phantomDriver.Manage().Cookies.AddCookie(cookie);
    phantomDriver.Navigate().GoToUrl("https://twitter.com/FOMO_Bears/");
    phantomDriver
        .FindElement(By.XPath("//div[contains(@aria-label, 'Follow') and contains(@role, 'button')]"), 10)
        .Click();

    phantomDriver.Manage().Cookies.DeleteAllCookies();
}

Thread.Sleep(-1);

public static class WebDriverExtensions
{
    public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
    {
        IWebElement result = null;
        var durations = 0;

        while (result == null)
        {
            try
            {
                result = driver.FindElement(by);
                break;
            }
            catch (NoSuchElementException)
            {
                durations += 100;
                Thread.Sleep(100);
            }

            if (durations > timeoutInSeconds * 1000)
                throw new NoSuchElementException();
        }

        return result;
    }
}

public class PhantomChromeDriver : ChromeDriver
{
    public PhantomChromeDriver() : base(RemovingFlag()) { }
    public PhantomChromeDriver(ChromeOptions options) : base(RemovingFlag(options)) { }

    private static ChromeOptions RemovingFlag() => ModifyOptions(new ChromeOptions());
    private static ChromeOptions RemovingFlag(ChromeOptions options) => ModifyOptions(options);

    private static ChromeOptions ModifyOptions(ChromeOptions options)
    {
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalChromeOption("useAutomationExtension", false);
        options.AddAdditionalOption("useAutomationExtension", false);
        options.AddArgument("--disable-infobars");
        options.AddArgument("--disable-blink-features=AutomationControlled");

        return options;
    }
}