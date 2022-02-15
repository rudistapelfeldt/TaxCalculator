using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using TaxCalculator.UITests.Pages;

namespace TaxCalculator.UITests.Tests
{
    public class MainPageTests : AbstractSetup
    {
        public MainPageTests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void DidAppStart()
        {
            var mainPage = new MainPage();
            mainPage.AppStarted();
        }
    }
}
