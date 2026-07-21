using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CustomMethods;
using LoginPagePOMs;
using HomePagePOMs;
using YourCartPOMs;

// 5 bugovi najdeni, 5 popraveni
// plus najden uste eden posle fixot na TC16, no ne e popraven

namespace NewVersionTestsV2
{
    internal class NewVersionTestsV2
    {
        private IWebDriver driver;
        private LoginPagePOM loginPagePOM;
        private Methods CustomMethods;
        private WebDriverWait wait;
        private HomePagePOM homePagePOM;
        private YourCartPOM yourCartPOM;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("C:\\Users\\Aleksandar\\Desktop\\Final Project\\FinalProjectAppV2\\PROJECT_maison-storeV2.html");
            driver.Manage().Window.Maximize();
            CustomMethods = new Methods(driver);
            loginPagePOM = new LoginPagePOM(driver);
            homePagePOM = new HomePagePOM(driver);
            yourCartPOM = new YourCartPOM(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void TC11_SearchMatches_DescriptionText()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SearchButton.SendKeys("Belgian");
            CustomMethods.ProductNameCheck(0, "Linen Throw Blanket");
            CustomMethods.ProductDescriptionCheck("Stonewashed Belgian linen in natural oat");
        }

        [Test]
        public void TC16_KitchenCategory_Filter() 
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[1].Click();
            CustomMethods.GridAndResultsCountCheck(4, "Showing 4 of 12 products");
            CustomMethods.CheckProductsOnCategory("7", "4", "1", "10");
            Assert.That(homePagePOM.CategoryFilters[0].GetAttribute("aria-pressed"), Is.EqualTo("false"));
        }

        [Test]
        public void TC24_SortingBy_NameAtoZ() 
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SortButtonSelect.SelectByValue("3");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "3", "8");
            homePagePOM.SortButtonSelect.SelectByValue("0");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "1", "12");
        }

        [Test]
        public void TC25_SortingWhenIn_CategoryTEST1() 
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[1].Click();
            homePagePOM.SortButtonSelect.SelectByValue("1");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[3], "data-product-id", "10", "1");
        }

        [Test]
        public void TC16_DecreaseQuantityToZeroShouldRemoveProduct()
        {
            CustomMethods.Login("jane_smith", "testing2025");
            homePagePOM.CorkDestMatAddToCart.Click();
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-12");
            CustomMethods.QuantityCheck("Cork Desk Mat", "1");
            CustomMethods.QuantityDecrease("Cork Desk Mat");
            Assert.That(yourCartPOM.CartItemsContainer[0].GetAttribute("data-testid"), Is.EqualTo("cart-empty-message"));
            yourCartPOM.CloseButton.Click();
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"open-cart-btn\"]/span")), Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}