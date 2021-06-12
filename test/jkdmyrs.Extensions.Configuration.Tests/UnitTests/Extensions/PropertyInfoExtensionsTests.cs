namespace jkdmyrs.Extensions.Configuration.Tests.UnitTests.Extensions
{
    using jkdmyrs.Extensions.Configuration.Attributes;
    using jkdmyrs.Extensions.Configuration.Extensions;
    using jkdmyrs.Testing.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory(TestCategories.Unit)]
    public class PropertyInfoExtensionsTests
    {
        private class AppSettings
        {
            public string BaseUrl { get; set; }

            public string SubscriptionKey { get; set; }
        }

        [AzureAppConfigKeyPrefix("EnterprisePricing")]
        private class PriceServiceSettings
        {
            [AzureAppConfigKey("BaseUrl")]
            public string BaseRequestUrl { get; set; }

            [AzureAppConfigSkipPrefix]
            [AzureAppConfigKeyPrefix("Apim")]
            [AzureAppConfigKey("SubKey")]
            public string SubscriptionKey { get; set; }

            [AzureAppConfigSkipPrefix]
            [AzureAppConfigKey("Authentication:AzureAd:ClientId")]
            public string ClientId { get; set; }

            [AzureAppConfigSkipPrefix]
            [AzureAppConfigKey("Authentication:AzureAd:ClientSecret")]
            public string ClientSecret { get; set; }
        }

        [TestMethod]
        public void NoAttributes_ClassNameAsPrefix()
        {
            var compareCount = 0;
            typeof(AppSettings).Properties().ForEach(property =>
            {
                if (property.Name == "BaseUrl")
                {
                    Assert.AreEqual("AppSettings:BaseUrl", property.FullKeyName<AppSettings>(true, ':'));
                    compareCount++;
                }
                if (property.Name == "SubscriptionKey")
                {
                    Assert.AreEqual("AppSettings:SubscriptionKey", property.FullKeyName<AppSettings>(true, ':'));
                    compareCount++;
                }
            });
            Assert.AreEqual(compareCount, typeof(AppSettings).Properties().Count);
        }

        [TestMethod]
        public void NoAttributes_ClassNameNotPrefix()
        {
            var compareCount = 0;
            typeof(AppSettings).Properties().ForEach(property =>
            {
                if (property.Name == "BaseUrl")
                {
                    Assert.AreEqual("BaseUrl", property.FullKeyName<AppSettings>(false, ':'));
                    compareCount++;
                }
                if (property.Name == "SubscriptionKey")
                {
                    Assert.AreEqual("SubscriptionKey", property.FullKeyName<AppSettings>(false, ':'));
                    compareCount++;
                }
            });
            Assert.AreEqual(compareCount, typeof(AppSettings).Properties().Count);
        }

        [TestMethod]
        public void Attributes()
        {
            var compareCount = 0;
            typeof(PriceServiceSettings).Properties().ForEach(property =>
            {
                if (property.Name == "BaseRequestUrl")
                {
                    Assert.AreEqual("EnterprisePricing:BaseUrl", property.FullKeyName<PriceServiceSettings>(false, ':'));
                    compareCount++;
                }
                if (property.Name == "SubscriptionKey")
                {
                    Assert.AreEqual("Apim:SubKey", property.FullKeyName<PriceServiceSettings>(false, ':'));
                    compareCount++;
                }
                if (property.Name == "ClientId")
                {
                    Assert.AreEqual("Authentication:AzureAd:ClientId", property.FullKeyName<PriceServiceSettings>(false, ':'));
                    compareCount++;
                }
                if (property.Name == "ClientSecret")
                {
                    Assert.AreEqual("Authentication:AzureAd:ClientSecret", property.FullKeyName<PriceServiceSettings>(false, ':'));
                    compareCount++;
                }
            });
            Assert.AreEqual(compareCount, typeof(PriceServiceSettings).Properties().Count);
        }
    }
}
