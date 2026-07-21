using OpenQA.Selenium;

namespace LoginPagePOMs
{
    internal class LoginPagePOM
    {
        public LoginPagePOM(IWebDriver _driver )
        {
            driver = _driver;
        }

        private readonly IWebDriver driver;

        public IWebElement LoginPage => driver.FindElement(By.XPath("//*[@data-testid='login-page']"));
        public IWebElement LoginHint => driver.FindElement(By.XPath("//*[@data-testid='login-hint']"));
        public IWebElement MaisonText => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[1]/div[4]/div[1]"));
        public IWebElement CuratedGoodsText => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[1]/div[4]/div[2]"));
        public IWebElement LoginForm => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[2]"));
        public IWebElement WelcomeBackText => driver.FindElement(By.XPath("//*[@data-testid='login-title']"));
        public IWebElement SignInText => driver.FindElement(By.XPath("//*[@id=\"login-form-container\"]/p[1]"));
        public IWebElement UsernameInput => driver.FindElement(By.XPath("//*[@id=\"username\"]"));
        public IWebElement PasswordInput => driver.FindElement(By.XPath("//*[@id=\"password\"]"));
        public IWebElement SignInButton => driver.FindElement(By.XPath("//*[@id=\"login-btn\"]"));
        public IWebElement LoginErrorMessage => driver.FindElement(By.XPath("//*[@id=\"login-form-container\"]/div"));
        public IWebElement ShowHideButton => driver.FindElement(By.XPath("//*[@id=\"login-form-container\"]/label[2]/div/button"));
        public IWebElement BrandQuote => driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[1]/div[4]/p"));
    }
}