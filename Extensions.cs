using OpenQA.Selenium;
using System.Collections.ObjectModel;

public static class WebDriverExtensions
{
    public static bool IsExist(this IWebElement element, By by)
    {
        try
        {
            element.FindElement(by);
            return true;
        }
        catch (Exception)
        {
            return false;            
        }
    }

    public static void CustomeClick(this IWebElement element, IWebDriver driver)
    {
        var js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].click();", element);
    }

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

    public static IWebElement FindElement(this IWebElement webElement, By by, int timeoutInSeconds)
    {
        IWebElement result = null;
        var durations = 0;

        while (result == null)
        {
            try
            {
                result = webElement.FindElement(by);
                break;
            }
            catch (NoSuchElementException)
            {
                durations += 100;
                Thread.Sleep(100);
            }
            catch (StaleElementReferenceException)
            {
                durations += 100;
                Thread.Sleep(100);
            }

            if (durations > timeoutInSeconds * 1000)
                throw new NoSuchElementException();
        }

        return result;
    }

    public static IEnumerable<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
    {
        ReadOnlyCollection<IWebElement> result = null;
        var durations = 0;

        while (result == null)
        {
            try
            {
                result = driver.FindElements(by);
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

    public static void WaitingForDispocal(this IWebDriver driver, By by, int timeoutInSeconds)
    {
        ReadOnlyCollection<IWebElement> result = null;
        var durations = 0;

        while (result == null)
        {
            try
            {
                result = driver.FindElements(by);
                durations += 100;
                Thread.Sleep(100);

            }
            catch (NoSuchElementException)
            {
                break;
            }

            if (durations > timeoutInSeconds * 1000)
                throw new Exception();
        }
    }
}

public class Extepsions
{

}
