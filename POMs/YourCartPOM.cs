using OpenQA.Selenium;

namespace YourCartPOMs
{ 
    internal class YourCartPOM
    {
        public YourCartPOM(IWebDriver _driver)
        {
            driver = _driver;
        }

        private readonly IWebDriver driver;

        public IWebElement YourCartText => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div[2]/h3"));
        public IWebElement CloseButton => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div[2]/button"));
        public IWebElement CartSideBar => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]")); 
        public IWebElement CheckoutButton => driver.FindElement(By.XPath("//*[@id=\"checkout-btn\"]"));
        public IWebElement TotalPrice => driver.FindElement(By.XPath("//*[@id=\"cart-total\"]"));
        public IWebElement TotalCount => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div[4]/div/span[1]"));
        public IWebElement EmptyCartIcon => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div[3]/div/div"));
        public IWebElement EmptyCartMessage => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]/div[3]/div"));
        public IList<IWebElement> CartItemsContainer => driver.FindElements(By.XPath("//div[@data-testid='cart-items-container']/div"));
        public IList<IWebElement> OSBControls => driver.FindElements(By.XPath("//*[@id=\"root\"]/div/div[2]/div[3]/div/div[2]/div[3]"));

        public Dictionary<string, By> quantityCheck = new()
        {
            {"Ceramic Pour-Over Set", By.XPath("//span[@data-testid='cart-item-qty-1']") },
            {"Linen Throw Blanket", By.XPath("//span[@data-testid='cart-item-qty-2']") },
            {"Brass Desk Lamp", By.XPath("//span[@data-testid='cart-item-qty-3']") },
            {"Oak Serving Board", By.XPath("//span[@data-testid='cart-item-qty-4']") },
            {"Soy Candle Trio", By.XPath("//span[@data-testid='cart-item-qty-5']") },
            {"Leather Journal", By.XPath("//span[@data-testid='cart-item-qty-6']") },
            {"Stoneware Mug Set", By.XPath("//span[@data-testid='cart-item-qty-7']") },
            {"Wool Felt Coasters", By.XPath("//span[@data-testid='cart-item-qty-8']") },
            {"Walnut Pen Holder", By.XPath("//span[@data-testid='cart-item-qty-9']") },
            {"Cotton Apron", By.XPath("//span[@data-testid='cart-item-qty-10']") },
            {"Incense Holder", By.XPath("//span[@data-testid='cart-item-qty-11']") },
            {"Cork Desk Mat", By.XPath("//span[@data-testid='cart-item-qty-12']") }
        };

        public Dictionary<string, By> removeButton = new()
        {
            {"Ceramic Pour-Over Set", By.XPath("//button[@data-testid='remove-item-1']") },
            {"Linen Throw Blanket", By.XPath("//button[@data-testid='remove-item-2']") },
            {"Brass Desk Lamp", By.XPath("//button[@data-testid='remove-item-3']") },
            {"Oak Serving Board", By.XPath("//button[@data-testid='remove-item-4']") },
            {"Soy Candle Trio", By.XPath("//button[@data-testid='remove-item-5']") },
            {"Leather Journal", By.XPath("//button[@data-testid='remove-item-6']") },
            {"Stoneware Mug Set", By.XPath("//button[@data-testid='remove-item-7']") },
            {"Wool Felt Coasters", By.XPath("//button[@data-testid='remove-item-8']") },
            {"Walnut Pen Holder", By.XPath("//button[@data-testid='remove-item-9']") },
            {"Cotton Apron", By.XPath("//button[@data-testid='remove-item-10']") },
            {"Incense Holder", By.XPath("//button[@data-testid='remove-item-11']") },
            {"Cork Desk Mat", By.XPath("//button[@data-testid='remove-item-12']") }
        };

        public Dictionary<string, By> increaseButtons = new()
        {
            {"Ceramic Pour-Over Set", By.XPath("//button[@data-testid='qty-increase-1']") },
            {"Linen Throw Blanket", By.XPath("//button[@data-testid='qty-increase-2']") },
            {"Brass Desk Lamp", By.XPath("//button[@data-testid='qty-increase-3']") },
            {"Oak Serving Board", By.XPath("//button[@data-testid='qty-increase-4']") },
            {"Soy Candle Trio", By.XPath("//button[@data-testid='qty-increase-5']") },
            {"Leather Journal", By.XPath("//button[@data-testid='qty-increase-6']") },
            {"Stoneware Mug Set", By.XPath("//button[@data-testid='qty-increase-7']") },
            {"Wool Felt Coasters", By.XPath("//button[@data-testid='qty-increase-8']") },
            {"Walnut Pen Holder", By.XPath("//button[@data-testid='qty-increase-9']") },
            {"Cotton Apron", By.XPath("//button[@data-testid='qty-increase-10']") },
            {"Incense Holder", By.XPath("//button[@data-testid='qty-increase-11']") },
            {"Cork Desk Mat", By.XPath("//button[@data-testid='qty-increase-12']") }
        };

        public Dictionary<string, By> decreaseButtons = new()
        {
            {"Ceramic Pour-Over Set", By.XPath("//button[@data-testid='qty-decrease-1']") },
            {"Linen Throw Blanket", By.XPath("//button[@data-testid='qty-decrease-2']") },
            {"Brass Desk Lamp", By.XPath("//button[@data-testid='qty-decrease-3']") },
            {"Oak Serving Board", By.XPath("//button[@data-testid='qty-decrease-4']") },
            {"Soy Candle Trio", By.XPath("//button[@data-testid='qty-decrease-5']") },
            {"Leather Journal", By.XPath("//button[@data-testid='qty-decrease-6']") },
            {"Stoneware Mug Set", By.XPath("//button[@data-testid='qty-decrease-7']") },
            {"Wool Felt Coasters", By.XPath("//button[@data-testid='qty-decrease-8']") },
            {"Walnut Pen Holder", By.XPath("//button[@data-testid='qty-decrease-9']") },
            {"Cotton Apron", By.XPath("//button[@data-testid='qty-decrease-10']") },
            {"Incense Holder", By.XPath("//button[@data-testid='qty-increase-11']") },
            {"Cork Desk Mat", By.XPath("//button[@data-testid='qty-decrease-12']") }
        };
    }
}