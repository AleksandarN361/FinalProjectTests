using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CustomMethods;
using LoginPagePOMs;
using HomePagePOMs;
using YourCartPOMs;

// 80 testa na Azure DevOps
// 27 vkupno za HomePage, 21 napraveni

namespace HomePageTests
{
    internal class HomePageTests
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
            driver.Navigate().GoToUrl("C:\\Users\\Aleksandar\\Desktop\\Final Project\\FinalProjectApp\\PROJECT maison-store.html");
            driver.Manage().Window.Maximize();
            CustomMethods = new Methods(driver);
            loginPagePOM = new LoginPagePOM(driver);
            homePagePOM = new HomePagePOM(driver);
            yourCartPOM = new YourCartPOM(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void TC01_AllProductsDisplayed_AfterLogin()
        {
            CustomMethods.Login("student", "learn2test");
            Assert.That(homePagePOM.AllButton.GetAttribute("aria-pressed"), Is.EqualTo("true"));
            CustomMethods.SubtitleAndHeadingCheck("Thoughtfully crafted essentials for everyday living", "Our Collection");
            CustomMethods.GridAndResultsCountCheck(12, "Showing 12 of 12 products");
        }

        [Test]
        public void TC04_ContentNavigationBar()
        {
            CustomMethods.Login("student", "learn2test");
            CustomMethods.CheckVisibilityAndText(homePagePOM.MaisonText, "Maison");
            CustomMethods.CheckVisibilityAndText(homePagePOM.WelcomeMessage, "Hello, Student");
            CustomMethods.CheckVisibilityAndText(homePagePOM.CartButton, "🛒");
            CustomMethods.CheckVisibility(homePagePOM.SignOutButton);
        }

        [Test]
        public void TC06_SearchBy_ExactProductName()
        {
            CustomMethods.Login("john_doe", "welcome1");
            CustomMethods.ButtonPressCheck(homePagePOM.AllButton, "aria-pressed", "true");
            homePagePOM.SearchButton.SendKeys("Brass Desk Lamp");
            CustomMethods.ProductNameCheck(0, "Brass Desk Lamp");
            CustomMethods.GridAndResultsCountCheck(1, "Showing 1 of 12 products");
        }

        [Test]
        public void TC07_SearchBy_PartialName()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SearchButton.SendKeys("candle");
            CustomMethods.ProductNameCheck(0, "Soy Candle Trio");
        }

        [Test]
        public void TC08_SearchCaseSensitive()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SearchButton.SendKeys("LEATHER");
            CustomMethods.ProductNameCheck(0, "Leather Journal");
        }

        [Test] 
        public void TC11_SearchMatches_DescriptionText() // Na ovoj test e pronajden bug, da se testira pak isto so v2
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SearchButton.SendKeys("Belgian");
            CustomMethods.ProductNameCheck(0, "Linen Throw Blanket");
            CustomMethods.ProductDescriptionCheck("Stonewashed Belgian linen in natural oat");
        }

        [Test]
        public void TC12_SearchMatches_DescriptionKeyword()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SearchButton.SendKeys("walnut");
            CustomMethods.ProductNameCheck(0, "Walnut Pen Holder");
        }

        [Test]
        public void TC13_NoMatchingProducts()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SearchButton.SendKeys("xyznonexistent");
            Assert.That(homePagePOM.ProductGrid.GetAttribute("data-testid"), Is.EqualTo("no-results"));
            Assert.That(homePagePOM.NoProductsMessage.Text, Is.EqualTo("No products found matching your criteria"));
            Assert.That(homePagePOM.NoProductsIcon.Text, Is.EqualTo("🔍"));
        }

        [Test]
        public void TC14_ProductsRestore_WhenSearchIsCleared()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SearchButton.SendKeys("xyznonexistent");
            CustomMethods.NoResultsCheck();
            homePagePOM.SearchButton.SendKeys(Keys.Control + "a");
            homePagePOM.SearchButton.SendKeys(Keys.Backspace);
            CustomMethods.GridAndResultsCountCheck(12, "Showing 12 of 12 products");
        }

        [Test]
        public void TC15_AllSelected_AfterLogin()
        {
            CustomMethods.Login("john_doe", "welcome1");
            Assert.That(homePagePOM.CategoryFilters[0].GetAttribute("aria-pressed"), Is.EqualTo("true"));
        }

        [Test]
        public void TC16_KitchenCategory_Filter() // Na ovoj test e pronajden bug, da se testira pak isto so v2
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[1].Click();
            CustomMethods.GridAndResultsCountCheck(4, "Showing 4 of 12 products");
            CustomMethods.CheckProductsOnCategory("7", "4", "1", "10");
            Assert.That(homePagePOM.CategoryFilters[0].GetAttribute("aria-pressed"), Is.EqualTo("false"));
        }

        [Test]
        public void TC17_LivingCategory_Filter()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[2].Click();
            CustomMethods.GridAndResultsCountCheck(4, "Showing 4 of 12 products");
            CustomMethods.CheckProductsOnCategory("2", "5", "8", "11");
        }

        [Test]
        public void TC18_OfficeCategory_Filter()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[3].Click();
            CustomMethods.GridAndResultsCountCheck(4, "Showing 4 of 12 products");
            CustomMethods.CheckProductsOnCategory("3", "6", "9", "12");
        }

        [Test]
        public void TC19_ReturnToAll_AfterFiltering()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[3].Click();
            CustomMethods.CheckProductsOnCategory("3", "6", "12", "9");
            homePagePOM.CategoryFilters[0].Click();
            Assert.That(homePagePOM.CategoryFilters[0].GetAttribute("aria-pressed"), Is.EqualTo("true"));
            CustomMethods.GridAndResultsCountCheck(12, "Showing 12 of 12 products");
        }

        [Test]
        public void TC20_SearchInCategory_Filtered()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[3].Click();
            CustomMethods.CheckProductsOnCategory("3", "6", "9", "12");
            homePagePOM.SearchButton.SendKeys("desk");
            CustomMethods.CheckProductsOnCategory("12", "3");
            CustomMethods.GridAndResultsCountCheck(2, "Showing 2 of 12 products");
        }

        [Test]
        public void TC22_PriceLowToHigh_Sorting()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SortButtonSelect.SelectByValue("1");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "8", "3");
            homePagePOM.SortButtonSelect.SelectByValue("0");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "1", "12");
        }

        [Test]
        public void TC23_PriceHighToLow_Sorting()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SortButtonSelect.SelectByValue("2");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "3", "8");
            homePagePOM.SortButtonSelect.SelectByValue("0");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "1", "12");
        }

        [Test]
        public void TC24_SortingBy_NameAtoZ() // Na ovoj test e pronajden bug, da se testira pak isto so v2
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SortButtonSelect.SelectByValue("3");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "3", "8");
            homePagePOM.SortButtonSelect.SelectByValue("0");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "1", "12");
        }

        [Test]
        public void TC25_SortingWhenIn_CategoryTEST1() // Na ovoj test e pronajden bug, da se testira pak isto so v2
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[1].Click();
            homePagePOM.SortButtonSelect.SelectByValue("1");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[3], "data-product-id", "10", "1");
        }

        [Test]
        public void TC26_SortingWhenIn_CategoryTEST2()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.CategoryFilters[3].Click();
            homePagePOM.SortButtonSelect.SelectByValue("1");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[3], "data-product-id", "9", "3");
        }

        [Test]
        public void TC27_SortPersistsWhile_SwitchingCategories()
        {
            CustomMethods.Login("john_doe", "welcome1");
            homePagePOM.SortButtonSelect.SelectByValue("2");
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[11], "data-product-id", "3", "8");
            homePagePOM.CategoryFilters[2].Click();
            CustomMethods.SortingCheck(homePagePOM.ProductGridDivs[0], homePagePOM.ProductGridDivs[3], "data-product-id", "2", "8");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}