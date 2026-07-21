using HomePagePOMs;
using LoginPagePOMs;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using YourCartPOMs;

namespace CustomMethods
{
    internal class Methods
    {
        private readonly IWebDriver driver;
        private readonly LoginPagePOM _loginpage;
        private readonly WebDriverWait _wait;
        private readonly HomePagePOM _homepage;
        private readonly YourCartPOM _yourcart;

        public Methods(IWebDriver _driver)
        {
            driver = _driver;
            _loginpage = new LoginPagePOM(driver);
            _homepage = new HomePagePOM(driver);
            _yourcart = new YourCartPOM(driver);
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }
    
        public void Login(IWebDriver driver, string username, string password)
        {
            _loginpage.UsernameInput.SendKeys(username);
            _loginpage.PasswordInput.SendKeys(password);
            _loginpage.SignInButton.Click();
        }

        public void Login(string username, string password)
        {
            _loginpage.UsernameInput.SendKeys(username);
            _loginpage.PasswordInput.SendKeys(password);
            _loginpage.SignInButton.Click();
        }

        public void ErrorTypeCheckAndUserStaysOnPage(string message, string errortype, string errortypetext, string title)
        {
            Assert.That(_loginpage.LoginErrorMessage.Text, Is.EqualTo(message));
            Assert.That(_loginpage.LoginErrorMessage.GetAttribute(errortype), Does.Contain(errortypetext));
            Assert.That(driver.Title, Is.EqualTo(title));
        }

        public void ErrorTypeCheckColorAndUserStaysOnPage(string message, string errortype, string errortypetext, string color, string colortype, string title)
        {
            Assert.That(_loginpage.LoginErrorMessage.Text, Is.EqualTo(message));
            Assert.That(_loginpage.LoginErrorMessage.GetAttribute(errortype), Does.Contain(errortypetext));
            Assert.That(_loginpage.LoginErrorMessage.GetCssValue(color), Does.Contain(colortype));
            Assert.That(driver.Title, Is.EqualTo(title));
        }

        public void SubtitleAndHeadingCheck(string subtitletext, string headingtext)
        {
            Assert.That(_homepage.HPSubtitle.Displayed);
            Assert.That(_homepage.HPSubtitle.Text, Is.EqualTo(subtitletext));
            Assert.That(_homepage.HPHeading.Displayed);
            Assert.That(_homepage.HPHeading.Text, Is.EqualTo(headingtext));
        }

        public void ProductNameCheck(int index, string text)
        {
            var product = _homepage.ProductGridDivs[index];
            var title = product.FindElement(By.TagName("h3"));
            Assert.IsTrue(product.Displayed);
            Assert.That(title.Text, Is.EqualTo(text));
        }

        public void NoResultsCheck()
        {
            Assert.That(_homepage.ProductGrid.GetAttribute("data-testid"), Is.EqualTo("no-results"));
            Assert.Multiple(() =>
            {
                Assert.That(_homepage.NoProductsMessage.Displayed);
                Assert.That(_homepage.NoProductsMessage.Text, Is.EqualTo("No products found matching your criteria"));
            });
            Assert.Multiple(() =>
            {
                Assert.That(_homepage.NoProductsIcon.Displayed);
                Assert.That(_homepage.NoProductsIcon.Text, Is.EqualTo("🔍"));
            });
        }

        // so params mozheme da pratime neogranicen broj na argumenti bez da pravime lista
        // a so Is.EquivalentTo ne treba po redosled da gi pisuvame nestata, za raslika od Is.EqualTo
        public void CheckProductsOnCategory(params string[] expectedIds)
        {
            var actualIds = _homepage.ProductGridDivs.Select(x => x.GetAttribute("data-product-id")).ToList();
            Assert.That(actualIds, Is.EquivalentTo(expectedIds));
        }

        public void CheckCartProducts(params string[] expectedCards)
        {
            var actualCards = _yourcart.CartItemsContainer.Select(x => x.GetAttribute("data-testid")).ToList();
            Assert.That(actualCards, Is.EquivalentTo(expectedCards));
        }

        public void CategoryFilterCheck(int index, string category)
        {
            var button = _homepage.CategoryFilters[index];
            var filter = button.GetAttribute("data-category");
            Assert.That(filter, Does.Contain(category));
        }

        public void ProductDescriptionCheck(string text)
        {
            var product = _homepage.ProductGridDivs[0];
            var description = product.FindElement(By.TagName("p"));
            Assert.That(description.Text, Is.EqualTo(text));
        }

        public void GridAndResultsCountCheck(int expectedcount, string count)
        {
            int actualcount = _homepage.ProductGridDivs.Count;
            Assert.That(actualcount, Is.EqualTo(expectedcount));
            Assert.That(_homepage.ResultsCount.Text, Is.EqualTo(count));
        }

        public void CheckVisibility(IWebElement element)
        {
            Assert.IsTrue(element.Displayed);
        }

        public void ButtonPressCheck(IWebElement element, string button, string situation)
        {
            Assert.That(element.GetAttribute(button), Is.EqualTo(situation));
        }

        public void CheckVisibilityAndText(IWebElement element, string text)
        {
            Assert.IsTrue(element.Displayed);
            Assert.That(element.Text, Is.EqualTo(text));
        }

        public void CheckAttribute(IWebElement element, string attribute, string text)
        {
            Assert.That(element.GetAttribute(attribute), Is.EqualTo(text));
        }

        public void CheckText(IWebElement element, string text)
        {
            Assert.That(element.Text, Is.EqualTo(text));
        }

        public bool ElementIsGone(By locator) //By se koristi za da najdeme sekad nov element, a ako ideme so IWebElement koristime vekje najden element
        {
            return driver.FindElements(locator).Count == 0;
        }

        public void SortingCheck(IWebElement first, IWebElement last, string attribute, string productcard1, string productcard2)
        {
            Assert.Multiple(() =>
            {
                Assert.That(first.Displayed);
                Assert.That(first.GetAttribute(attribute), Is.EqualTo(productcard1));
            });
            Assert.Multiple(() =>
            {
                Assert.That(last.Displayed);
                Assert.That(last.GetAttribute(attribute), Is.EqualTo(productcard2));
            });
        }

        public void ClickTwice(IWebElement element)
        {
            element.Click();
            element.Click();
        }

        public void TripleClick(IWebElement element)
        {
            element.Click();
            element.Click();
            element.Click();
        }

        public void QuantityIncrease(string productName)
        {
            driver.FindElement(_yourcart.increaseButtons[productName]).Click();
        }

        public void QuantityCheck(string productName, string expectedQuantity)
        {
            var actualQuantity = driver.FindElement(_yourcart.quantityCheck[productName]);
            Assert.That(expectedQuantity, Is.EqualTo(actualQuantity.Text));
        }

        public void RemoveProduct(string productName)
        {
            driver.FindElement(_yourcart.removeButton[productName]).Click();
        }

        public void QuantityDecrease(string productName)
        {
            driver.FindElement(_yourcart.decreaseButtons[productName]).Click();
        }
    }
}