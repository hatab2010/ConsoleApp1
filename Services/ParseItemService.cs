using ConsoleApp1.Data;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

public class ChechItemService
{
    const string FAVORITE_PAGE = "https://sbermegamarket.ru/personal/favorites/";
    private IWebDriver _driver;
    private IServiceProvider _serviceProvider;
    public ChechItemService
        (
            WorkerService workerService,
            IServiceProvider serviceProvider
        )
    {
        _driver = workerService.GetDriver();
        _serviceProvider = serviceProvider;
    }

    public void Check()
    {
        List<Item> ProductItems;

        _driver.Navigate().GoToUrl(FAVORITE_PAGE);

        try
        {
            _driver.FindElement(By.CssSelector(".loading"), 3);
            _driver.WaitingForDispocal(By.CssSelector(".loading"), 3);
            Thread.Sleep(300); //TODO
        }
        catch (NoSuchElementException)
        {

        }

        WebItem.Find(_driver);

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<DomainDbContext>();
            ProductItems = context.Items.Where(_ => _.IsChecked).ToList();

            var webItems = WebItem.FindAll(_driver);

            foreach (var webItem in webItems)
            {
                var ctxItem = context.Items.FirstOrDefault(_ => _.DataId == webItem.Id);

                if (ctxItem == null || ctxItem.IsChecked)
                {
                    webItem.Like();
                    continue;
                }

                webItem.Like();
                if (webItem.Price > 70)
                {
                    ctxItem.IsAbuse = true;
                }

                ctxItem.IsChecked = true;
                context.SaveChanges();
            }
        }
    }
}

public class ParseItemService
{
    private IWebDriver _driver;
    private IServiceProvider _serviceProvider;

    public ParseItemService
        (
            WorkerService workerService,
            IServiceProvider provider
        )
    {
        _driver = workerService.GetDriver();
        _serviceProvider = provider;
    }

    public void Check()
    {

    }

    public void ParseItems(int maxPrice)
    {
        Category category = null;

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<DomainDbContext>();
            category = context.Categories.First();

            while (true)
            {
                _driver.Navigate()
                    .GoToUrl($"{category.Uri}&page={++category.LastParsePage}");

                try
                {
                    _driver.FindElement(By.CssSelector(".loading"), 3);
                    _driver.WaitingForDispocal(By.CssSelector(".loading"), 3);
                    Thread.Sleep(300); //TODO
                }
                catch (NoSuchElementException)
                {

                }

                WebItem.Find(_driver);
                var items = WebItem.FindAll(_driver);

                foreach (var item in items)
                {
                    if (!item.IsLike)
                        item.Like();

                    if (item.Price > maxPrice)
                    {
                        continue;
                    }

                    var ctxCategory = context.Categories.First(_ => _.Id.ToString() == category.Id.ToString());

                    var value = new Item()
                    {
                        DataId = item.Id,
                        Category = ctxCategory,
                        Uri = item.Uri,
                        CategoryId = ctxCategory.Id,
                        Name = item.Title,
                        Price = item.Price,
                        IsFavorite = true
                    };

                    var ctxItem = context.Items.FirstOrDefault(_ => _.Uri == item.Uri);

                    if (ctxItem == null)
                        context.Items.Add(value);
                }

                context.SaveChanges();
            }

        }
    }
}
