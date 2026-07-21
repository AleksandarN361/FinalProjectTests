using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CustomMethods;
using LoginPagePOMs;
using HomePagePOMs;
using YourCartPOMs;
using SeleniumExtras.WaitHelpers;

// 80 testa na Azure DevOps
// 23 vkupno za LoginPage, 21 napraveni (24 pokazuva poradi 4 TestCase od TC20) 

namespace LoginPageTests
{
    public class LoginPageTests
    {
        public IWebDriver driver;
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
        public void TC01_LoginPage_AppStart() 
        {
            Assert.That(loginPagePOM.MaisonText.Text, Is.EqualTo("Maison"));
            Assert.That(loginPagePOM.CuratedGoodsText.Text, Is.EqualTo("Curated Goods".ToUpper()));
            Assert.IsTrue(loginPagePOM.LoginPage.Displayed);
        }

        [Test]
        public void TC02_LoginForm_ElementsVisibility()
        {
            CustomMethods.CheckVisibilityAndText(loginPagePOM.WelcomeBackText, "Welcome back");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.SignInText, "Sign in to your account");
            Assert.IsTrue(loginPagePOM.UsernameInput.Displayed);
            Assert.IsTrue(loginPagePOM.PasswordInput.Displayed);
            Assert.IsTrue(loginPagePOM.SignInButton.Displayed);
            Assert.IsTrue(loginPagePOM.LoginHint.Displayed);
        }

        [Test]
        public void TC04_HiddenPassword_ByDefault()
        {
            loginPagePOM.PasswordInput.SendKeys("mypassword");
            Assert.That(loginPagePOM.PasswordInput.GetAttribute("type"), Is.EqualTo("password"));
            Assert.IsTrue(loginPagePOM.ShowHideButton.Displayed);
        }

        [Test]
        public void TC05_RevealPassword()
        {
            loginPagePOM.PasswordInput.SendKeys("password123");
            Assert.That(loginPagePOM.PasswordInput.GetAttribute("type"), Is.EqualTo("password"));
            loginPagePOM.ShowHideButton.Click();
            Assert.That(loginPagePOM.PasswordInput.GetAttribute("type"), Is.EqualTo("text"));
            Assert.That(loginPagePOM.ShowHideButton.Text, Is.EqualTo("Hide"));
        }

        [Test]
        public void TC06_HidePassword_AfterReveal()
        {
            loginPagePOM.PasswordInput.SendKeys("character");
            loginPagePOM.ShowHideButton.Click();
            CustomMethods.CheckAttribute(loginPagePOM.PasswordInput, "type", "text");
            CustomMethods.CheckText(loginPagePOM.ShowHideButton, "Hide");
            loginPagePOM.ShowHideButton.Click();
            CustomMethods.CheckAttribute(loginPagePOM.PasswordInput, "type", "password");
            CustomMethods.CheckText(loginPagePOM.ShowHideButton, "Show");
        }

        [Test]
        public void TC07_BothFields_Empty()
        {
            //Assert.That(driver.Title, Is.EqualTo("Maison — Sign In")); dali treba i ova ? ili ne, za da se osigurame deka ostanuvame na ista strana, prvo proveruvame deka sme na taa !
            loginPagePOM.SignInButton.Click();
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Please fill in both fields");
            Assert.That(loginPagePOM.LoginErrorMessage.GetAttribute("data-error-type"), Is.EqualTo("empty-fields"));
            Assert.That(driver.Title, Is.EqualTo("Maison — Sign In"));
        }

        [Test]
        public void TC08_UsernameEmpty_PasswordFilled()
        {
            CustomMethods.Login("", "somepassword");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Please fill in both fields");
            CustomMethods.CheckAttribute(loginPagePOM.LoginErrorMessage, "data-error-type", "empty-fields");
        }

        [Test]
        public void TC09_PasswordEmpty_UsernameFilled()
        {
            loginPagePOM.SignInButton.Click();
            loginPagePOM.UsernameInput.SendKeys("someuser");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Please fill in both fields");
            Assert.That(loginPagePOM.LoginErrorMessage.GetAttribute("data-error-type"), Is.EqualTo("empty-fields"));
        }

        [Test]
        public void TC10_BothFields_WhitespaceOnly()
        {
            CustomMethods.Login(" ", " ");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Please fill in both fields");
            CustomMethods.CheckAttribute(loginPagePOM.LoginErrorMessage, "data-error-type", "empty-fields");
        }

        [Test]
        public void TC11_WrongUsername()
        {
            CustomMethods.Login("nonexistent_user", "anypassowrd");
            wait.Until(driver => loginPagePOM.LoginErrorMessage.Displayed);
            CustomMethods.ErrorTypeCheckAndUserStaysOnPage("Invalid username or password. Please try again.", "data-error-type", "invalid-credentials", "Maison — Sign In");
        }

        [Test]
        public void TC12_CorrectUsername_WrongPassword()
        {
            CustomMethods.Login("admin", "wrongpassword");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Invalid username or password. Please try again.");
            CustomMethods.CheckAttribute(loginPagePOM.LoginErrorMessage, "data-error-type", "invalid-credentials");
        }

        [Test]
        public void TC13_CaseSensitive_Password()
        {
            CustomMethods.Login("student", "Learn2Test");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Invalid username or password. Please try again.");
            Assert.That(loginPagePOM.LoginErrorMessage.GetAttribute("data-error-type"), Is.EqualTo("invalid-credentials"));
        }

        [Test]
        public void TC14_BlockedUser_Attempt()
        {
            CustomMethods.Login("bob_blocked", "blocked99");
            wait.Until(driver => loginPagePOM.LoginErrorMessage.Displayed);
            CustomMethods.ErrorTypeCheckColorAndUserStaysOnPage("⚠This account has been suspended. Please contact support for assistance.", "data-error-type", "account-blocked", "color", "rgba(196, 154, 96, 1)", "Maison — Sign In");
        }

        [Test]
        public void TC15_BlockedUser_WrongPassword()
        {
            CustomMethods.Login("bob_blocked", "wrongpassword");
            CustomMethods.Login("admin", "wrongpassword");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Invalid username or password. Please try again.");
            CustomMethods.CheckAttribute(loginPagePOM.LoginErrorMessage, "data-error-type", "invalid-credentials");
        }

        [Test]
        public void TC16_LoginWith_ValidAccount()
        {
            CustomMethods.Login("student", "learn2test");
            Assert.That(loginPagePOM.SignInButton.Text, Is.EqualTo("Signing in...".ToUpper()));
            wait.Until(driver => driver.Title == "Maison — Products");
            Assert.That(driver.Title, Is.EqualTo("Maison — Products"));
        }

        [Test]
        public void TC17_CaseInsensitive_Username()
        {
            CustomMethods.Login("ADMIN", "admin123");
            wait.Until(driver => driver.Title == "Maison — Products");
            Assert.That(driver.Title, Is.EqualTo("Maison — Products"));
            CustomMethods.CheckVisibilityAndText(homePagePOM.WelcomeMessage, "Hello, Admin");
        }

        [Test]
        public void TC19_LoginWith_EnterKeyTEST2()
        {
            loginPagePOM.UsernameInput.SendKeys("john_doe");
            loginPagePOM.PasswordInput.SendKeys("welcome1");
            loginPagePOM.PasswordInput.SendKeys(Keys.Enter);
            wait.Until(driver => driver.Title == "Maison — Products");
        }

        [TestCase("admin", "admin123", "Hello, Admin")]  
        [TestCase("john_doe", "welcome1", "Hello, John")]
        [TestCase("jane_smith", "testing2025", "Hello, Jane")]
        [TestCase("student", "learn2test", "Hello, Student")]
        public void TC20_WelcomeMessage_FromUsername(string username, string password, string expectedMessage)
        {
            CustomMethods.Login(username, password);
            string actualMessage = homePagePOM.WelcomeMessage.Text;
            Assert.That(expectedMessage, Is.EqualTo(actualMessage));
        }

        [Test]
        public void TC21_NewAttempt_ClearsError() // testot proagja, no dali ova so InvisibilityOfElementsLocated raboti ???
        {
            CustomMethods.Login("nouser", "nopassword");
            CustomMethods.CheckVisibilityAndText(loginPagePOM.LoginErrorMessage, "Invalid username or password. Please try again.");
            loginPagePOM.UsernameInput.SendKeys(Keys.Control + "a");
            loginPagePOM.UsernameInput.SendKeys(Keys.Backspace);
            loginPagePOM.PasswordInput.SendKeys(Keys.Control + "a");
            loginPagePOM.PasswordInput.SendKeys(Keys.Backspace);
            CustomMethods.Login("student", "learn2test");
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"login-form-container\"]/div"))); // ceka porakata da iscezne (elementot) od DOM, pred da se prefrli na HomePage
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"login-form-container\"]/div")), Is.True);
        }

        [Test]
        public void TC22_SignOutFrom_HomePage()
        {
            CustomMethods.Login("student", "learn2test");
            homePagePOM.SignOutButton.Click();
            Assert.That(driver.Title, Is.EqualTo("Maison — Sign In"));
            Assert.That(loginPagePOM.UsernameInput.GetAttribute("value"), Is.Empty);
            Assert.That(loginPagePOM.PasswordInput.GetAttribute("value"), Is.Empty);
        }

        [Test]
        public void TC23_CartCleared_OnSignOut()
        {
            CustomMethods.Login("student", "learn2test");
            homePagePOM.CeramicPourOverSetAddToCart.Click();
            homePagePOM.LinenThrowBlanketAddToCart.Click();
            homePagePOM.CartButton.Click();
            Assert.That(yourCartPOM.CartItemsContainer.Count, Is.EqualTo(2));
            yourCartPOM.CloseButton.Click();
            homePagePOM.SignOutButton.Click();
            CustomMethods.Login("student", "learn2test");
            homePagePOM.CartButton.Click();
            Assert.That(yourCartPOM.CartItemsContainer.Count, Is.EqualTo(1)); // ova e 1 poradi empty message porakata, bidejki i toa e div 
            yourCartPOM.CloseButton.Click();
            Assert.That(CustomMethods.ElementIsGone(By.XPath("//*[@id=\"open-cart-btn\"]/span")), Is.True); // ??? Dali e okej vaka ?
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}