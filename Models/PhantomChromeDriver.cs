using OpenQA.Selenium.Chrome;

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