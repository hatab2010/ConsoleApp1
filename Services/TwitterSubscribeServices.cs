using ConsoleApp1;
using OpenQA.Selenium;

public class TwitterSubscribeServices
{
    public void Start()
    {
        var phantomDriver = new PhantomChromeDriver();
        phantomDriver.Navigate().GoToUrl("https://sbermegamarket.ru/");

        foreach (var token in Global.TwitterToke)
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
    }
}
