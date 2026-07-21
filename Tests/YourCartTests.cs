using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CustomMethods;
using LoginPagePOMs;
using HomePagePOMs;
using YourCartPOMs;

// 80 testa na Azure DevOps
// 23 vkupno za YourCart, 19 napraveni

namespace YourCartTests
{
    internal class YourCartTests
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
            CustomMethods.Login(driver, "jane_smith", "testing2025");
        }

        [Test]
        public void TC02_AddMultiple_DifferentProducts_CartBadge()
        {
            homePagePOM.CeramicPourOverSetAddToCart.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("1"));
            homePagePOM.LeatherJournalAddToCart.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("2"));
        }

        [Test]
        public void TC03_AddMultiple_SameProducts_CartBadge()
        {
            homePagePOM.BrassDeskLampAddToCart.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("1"));
            homePagePOM.BrassDeskLampAddToCart.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("2"));
            homePagePOM.CartButton.Click();
            Assert.That(yourCartPOM.CartItemsContainer.Count, Is.EqualTo(1));
            CustomMethods.CheckCartProducts("cart-item-3");
        }

        [Test]
        public void TC04_CartBadge_NotVisible_WhenCartEmptyTEST1()
        {
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"open-cart-btn\"]/span")), Is.True);
        }

        [Test]
        public void TC06_OpenCartSidebar()
        {
            homePagePOM.CartButton.Click();
            Assert.That(yourCartPOM.CartSideBar.GetAttribute("style"), Does.Contain("background: rgb(21, 20, 19)"));
            Assert.That(yourCartPOM.YourCartText.Text, Is.EqualTo("Your Cart"));
            Assert.That(yourCartPOM.CloseButton.Text, Is.EqualTo("×"));
        }

        [Test]
        public void TC07_CloseCart_WithCloseButton()
        {
            homePagePOM.CartButton.Click();
            Assert.IsTrue(yourCartPOM.CartSideBar.Displayed);
            yourCartPOM.CloseButton.Click();
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"root\"]/div/div[2]")), Is.True); // ??? Dali e okej vaka ?
        }

        [Test]
        public void TC09_EmptyCartMessage()
        {
            homePagePOM.CartButton.Click();
            Assert.IsTrue(yourCartPOM.EmptyCartIcon.Displayed);
            Assert.Multiple(() =>
            {
                Assert.That(yourCartPOM.EmptyCartMessage.Displayed);
                Assert.That(yourCartPOM.EmptyCartMessage.Text, Does.Contain("Your cart is empty"));
            });
            var button = driver.FindElements(By.Id("checkout-btn"));
            var price = driver.FindElements(By.Id("cart-total"));
            Assert.That(button.Count, Is.EqualTo(0));
            Assert.That(price.Count, Is.EqualTo(0));
        }

        [Test]
        public void TC10_CartShows_AddedProductDetails()
        {
            homePagePOM.SoyCandleTrioAddToCart.Click();
            CustomMethods.CheckVisibilityAndText(homePagePOM.CartBadge, "1");
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-5");
        }

        [Test]
        public void TC12_IncreaseQuantity_OneTimeTEST1()
        {
            homePagePOM.LeatherJournalAddToCart.Click();
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-6");
            CustomMethods.QuantityCheck("Leather Journal", "1");
            CustomMethods.QuantityIncrease("Leather Journal");
            CustomMethods.QuantityCheck("Leather Journal", "2");
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$56.00"));
            yourCartPOM.CloseButton.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("2"));
        }

        [Test]
        public void TC13_IncreaseQuantity_OneTimeTEST2()
        {
            homePagePOM.LeatherJournalAddToCart.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("1"));
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-6");
            CustomMethods.QuantityCheck("Leather Journal", "1");
            yourCartPOM.CloseButton.Click();
            homePagePOM.LeatherJournalAddToCart.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("2"));
            homePagePOM.CartButton.Click();
            CustomMethods.QuantityCheck("Leather Journal", "2");
        }

        [Test]
        public void TC14_IncreaseQuantity_MultipleTimes()
        {
            homePagePOM.WoolFeltCoastersAddToCart.Click();
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-8");
            CustomMethods.QuantityCheck("Wool Felt Coasters", "1");
            CustomMethods.QuantityIncrease("Wool Felt Coasters");
            CustomMethods.QuantityIncrease("Wool Felt Coasters");
            CustomMethods.QuantityIncrease("Wool Felt Coasters");
            CustomMethods.QuantityCheck("Wool Felt Coasters", "4");
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$72.00"));
        }

        [Test]
        public void TC15_DecreaseQuantity_OneTime()
        {
            homePagePOM.CorkDestMatAddToCart.Click();
            CustomMethods.CheckVisibilityAndText(homePagePOM.CartBadge, "1");
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-12");
            CustomMethods.QuantityCheck("Cork Desk Mat", "1");
            CustomMethods.QuantityIncrease("Cork Desk Mat");
            CustomMethods.QuantityIncrease("Cork Desk Mat");
            CustomMethods.QuantityCheck("Cork Desk Mat", "3");
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$135.00"));
            CustomMethods.QuantityDecrease("Cork Desk Mat");
            CustomMethods.QuantityCheck("Cork Desk Mat", "2");
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$90.00"));
        }

        [Test]
        public void TC16_DecreaseQuantity_ToZero_ShouldRemoveProduct() // Na ovoj test e pronajden bug, da se testira pak isto so v2
        {
            homePagePOM.CorkDestMatAddToCart.Click();
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-12");
            CustomMethods.QuantityCheck("Cork Desk Mat", "1");
            CustomMethods.QuantityDecrease("Cork Desk Mat");
            Assert.That(yourCartPOM.CartItemsContainer[0].GetAttribute("data-testid"), Is.EqualTo("cart-empty-message"));
            yourCartPOM.CloseButton.Click();
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"open-cart-btn\"]/span")), Is.True);
        }

        [Test]
        public void TC17_RemoveItem_WithQuantityOne()
        {
            homePagePOM.IncenseHolderAddToCart.Click();
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-11");
            CustomMethods.QuantityCheck("Incense Holder", "1");
            CustomMethods.RemoveProduct("Incense Holder");
            yourCartPOM.CloseButton.Click();
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"open-cart-btn\"]/span")), Is.True);
        }

        [Test]
        public void TC18_RemoveItem_WithQuantityGreaterThanOne()
        {
            homePagePOM.StonewareMugSetAddToCart.Click();
            Assert.That(homePagePOM.CartBadge.Text, Is.EqualTo("1"));
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-7");
            CustomMethods.QuantityCheck("Stoneware Mug Set", "1");
            CustomMethods.QuantityIncrease("Stoneware Mug Set");
            CustomMethods.QuantityIncrease("Stoneware Mug Set");
            CustomMethods.QuantityIncrease("Stoneware Mug Set");
            CustomMethods.QuantityIncrease("Stoneware Mug Set");
            CustomMethods.QuantityCheck("Stoneware Mug Set", "5");
            CustomMethods.RemoveProduct("Stoneware Mug Set");
            Assert.That(yourCartPOM.CartItemsContainer[0].GetAttribute("data-testid"), Is.EqualTo("cart-empty-message"));
        }

        [Test]
        public void TC19_RemoveLastItem_IsCartEmpty()
        {
            homePagePOM.LinenThrowBlanketAddToCart.Click();
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-2");
            CustomMethods.QuantityCheck("Linen Throw Blanket", "1");
            CustomMethods.RemoveProduct("Linen Throw Blanket");
            Assert.That(yourCartPOM.CartItemsContainer[0].GetAttribute("data-testid"), Is.EqualTo("cart-empty-message"));
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"checkout-btn\"]")), Is.True);
            yourCartPOM.CloseButton.Click();
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"open-cart-btn\"]/span")), Is.True);
        }

        [Test]
        public void TC20_OneItemTotal()
        {
            homePagePOM.BrassDeskLampAddToCart.Click();
            homePagePOM.CartButton.Click();
            Assert.That(yourCartPOM.TotalCount.Text, Is.EqualTo("Total (1 items)"));
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$125.00"));
            Assert.IsTrue(yourCartPOM.CheckoutButton.Displayed);
        }

        [Test]
        public void TC21_MultipleItems_Total()
        {
            CustomMethods.ClickTwice(homePagePOM.CeramicPourOverSetAddToCart);
            homePagePOM.WoolFeltCoastersAddToCart.Click();
            homePagePOM.CartButton.Click();
            Assert.That(yourCartPOM.TotalCount.Text, Is.EqualTo("Total (3 items)"));
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$114.00"));
            Assert.IsTrue(yourCartPOM.CheckoutButton.Displayed);
        }

        [Test]
        public void TC22_TotalUpdates_OnQuantityChange()
        {
            homePagePOM.OakServingBoardAddToCart.Click();
            CustomMethods.CheckVisibilityAndText(homePagePOM.CartBadge, "1");
            homePagePOM.CartButton.Click();
            CustomMethods.CheckCartProducts("cart-item-4");
            CustomMethods.QuantityCheck("Oak Serving Board", "1");
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$36.00"));
            CustomMethods.QuantityIncrease("Oak Serving Board");
            CustomMethods.QuantityCheck("Oak Serving Board", "2");
            Assert.That(yourCartPOM.TotalPrice.Text, Is.EqualTo("$72.00"));
            Assert.That(yourCartPOM.TotalCount.Text, Is.EqualTo("Total (2 items)"));
        }

        [Test]
        public void TC23_CartBadgeReflect_TotalItemCount()
        {
            CustomMethods.ClickTwice(homePagePOM.LeatherJournalAddToCart);
            CustomMethods.TripleClick(homePagePOM.CottonApronAddToCart);
            CustomMethods.CheckVisibilityAndText(homePagePOM.CartBadge, "5");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}