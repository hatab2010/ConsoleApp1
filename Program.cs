
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;

IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<WorkerService>();
serviceCollection.AddSingleton<ParseItemService>();
//serviceCollection.AddScoped<ChechItemService>();
//serviceCollection.AddScoped<PhantomChromeDriver>();
serviceCollection.AddDbContext<DomainDbContext>(options => options.UseSqlite("Filename=C:\\Users\\cowor\\source\\repos\\ConsoleApp1\\ConsoleApp1\\Mobile.db"));


var serviceProvider = serviceCollection.BuildServiceProvider();
var parse = serviceProvider.GetService<ParseItemService>();
parse.ParseItems(50);

Thread.Sleep(-1);

public class WorkerService
{
    private PhantomChromeDriver _driver;

    public IWebDriver GetDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument($"--user-data-dir={Directory.GetCurrentDirectory()}/Main");
        return new PhantomChromeDriver(options);
    }
}

public class WebItem
{
    public string Id => _element.GetAttribute("data-product-id");
    public string Title => _element.FindElement(By.CssSelector(".item-title")).Text;
    public Uri Uri => new Uri(_element.FindElement(By.TagName("a")).GetAttribute("href"));
    public int Price => int.Parse(Regex.Match(_element.FindElement(By.CssSelector(".item-price")).Text, @"\d+").Value);
    public bool IsLike => _element.IsExist(By.CssSelector(".to-favorite-button svg"));


    const string XPath = "//div[@class = 'catalog-item']";

    private IWebElement _element;

    public static WebItem Find(IWebDriver driver)
    {
        return new WebItem() { _element = driver.FindElement(By.XPath(XPath), 3) };
    }

    public static IEnumerable<WebItem> FindAll(IWebDriver driver)
    {
        List<WebItem> items = new List<WebItem>();

        foreach (var item in driver.FindElements(By.XPath(XPath)))
            items.Add(new WebItem() { _element = item });

        return items;
    }

    public void Like()
    {
        var likeBtn = _element.FindElement(By.CssSelector(".to-favorite-button"));
        likeBtn.CustomeClick(((IWrapsDriver)_element).WrappedDriver);
    }
}
