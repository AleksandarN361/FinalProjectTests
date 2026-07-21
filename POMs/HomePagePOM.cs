using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace HomePagePOMs
{
    internal class HomePagePOM
    {
        public HomePagePOM(IWebDriver _driver)
        {
            driver = _driver;
        }

        private readonly IWebDriver driver;

        public IWebElement HPHeading => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/h1"));
        public IWebElement HPSubtitle => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/p"));
        public IWebElement ResultsCount => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div[3]"));
        public IWebElement AllButton => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div[1]/div[2]/button[1]"));
        public IList<IWebElement> ProductGridDivs => driver.FindElements(By.XPath("//div[@data-testid='product-grid']/div"));
        public IWebElement ProductGrid => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div[2]"));
        public IWebElement SearchButton => driver.FindElement(By.XPath("//*[@id=\"search-input\"]"));
        public IWebElement NoProductsMessage => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div[2]/p"));
        public IWebElement NoProductsIcon => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/div[2]/div"));
        public IWebElement SignOutButton => driver.FindElement(By.Id("sign-out"));
        public IWebElement WelcomeMessage => driver.FindElement(By.XPath("//*[@id=\"main-nav\"]/div[2]/span"));
        public IWebElement CartBadge => driver.FindElement(By.XPath("//*[@id=\"open-cart-btn\"]/span"));
        public IWebElement CartButton => driver.FindElement(By.XPath("//*[@id=\"open-cart-btn\"]"));
        public IWebElement MaisonText => driver.FindElement(By.XPath("//*[@id=\"main-nav\"]/div[1]"));
        public IList<IWebElement> CategoryFilters => driver.FindElements(By.XPath("//*[@id=\"root\"]/div/div/div[1]/div[2]/button"));
        public IWebElement SortButton => driver.FindElement(By.Id("sort-select"));
        public SelectElement SortButtonSelect => new SelectElement(SortButton);
        public IWebElement CeramicPourOverSetAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[1]/div[3]/button"));
        public IWebElement LinenThrowBlanketAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[2]/div[3]/button"));
        public IWebElement LeatherJournalAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[6]/div[3]/button"));
        public IWebElement BrassDeskLampAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[3]/div[3]/button"));
        public IWebElement SoyCandleTrioAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[5]/div[3]/button"));
        public IWebElement WoolFeltCoastersAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[8]/div[3]/button"));
        public IWebElement OakServingBoardAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[4]/div[3]/button"));
        public IWebElement CottonApronAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[10]/div[3]/button"));
        public IWebElement IncenseHolderAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[11]/div[3]/button"));
        public IWebElement CorkDestMatAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[12]/div[3]/button"));
        public IWebElement StonewareMugSetAddToCart => driver.FindElement(By.XPath("//*[@id=\"products-container\"]/div[7]/div[3]/button"));

        // PRODUCTS
        // Ceramic Pour-Over Set - 1
        // Linen Throw Blanket - 2
        // Brass Desk Lamp - 3
        // Oak Serving Board - 4
        // Soy Candle Trio - 5
        // Leather Journal - 6
        // Stoneware Mug Set - 7
        // Wool Felt Coasters - 8
        // Walnut Pen Holder - 9
        // Cotton Apron - 10
        // Incense Holder - 11
        // Cork Desk Mat - 12
    }
}