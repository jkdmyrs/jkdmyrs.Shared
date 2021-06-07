namespace jkdmyrs.Extensions.Configuration.Tests.IntegrationTests.Extensions
{
    using Azure.Identity;
    using FluentAssertions;
    using jkdmyrs.Extensions.Configuration.Attributes;
    using jkdmyrs.Extensions.Configuration.Extensions;
    using jkdmyrs.Extensions.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory(TestCategories.Integration)]
    public class ServiceCollectionExtensionsTests
    {
        private class ExampleSetting
        {
            public string TestConfigKey { get; set; }

            [AzureAppConfigKey("TestSettings:ExampleKey")]
            public string TestSecret { get; set; }
        }

        [AzureAppConfigKeyPrefix("OrderCapture")]
        private class ExampleSetting2
        {
            [AzureAppConfigKey("TestSetting2")]
            // full key name: OrderCapture:TestSetting2
            public string Version { get; set; }

            [AzureAppConfigKey("TestSetting")]
            // full key name: OrderCapture:TestSetting
            public string AppName { get; set; }

            [AzureAppConfigSkipPrefix]
            [AzureAppConfigKey("TestConfigKey")]
            // full key name: TestConfigKey
            public string ClientKey { get; set; }

            [AzureAppConfigKeyPrefix("Auth")]
            [AzureAppConfigKey("TestSetting3")]
            // full key name: OrderCapture:Auth:TestSetting3
            public string AuthKey { get; set; }
        }

        [TestMethod]
        public void CanGetKeys()
        {
            // setup a service provider
            var provider = new ServiceCollection()
                .AddAzureAppConfig(TestSettings.ConnectionString, new ClientSecretCredential(TestSettings.TenantId, TestSettings.ClientId, TestSettings.ClientSecret))
                .AddAppConfigSetting<ExampleSetting>()
                .AddAppConfigSetting<ExampleSetting2>()
                .BuildServiceProvider();

            // get the settings from the provider
            var settings = provider.GetService<ExampleSetting>();
            var settings2 = provider.GetService<ExampleSetting2>();

            // verify 
            settings.TestConfigKey.Should().Be("TestConfigValue");
            settings.TestSecret.Should().Be("TestConfigSecretValue");

            settings2.Version.Should().Be("123");
            settings2.AppName.Should().Be("test");
            settings2.ClientKey.Should().Be("TestConfigValue");
            settings2.AuthKey.Should().Be("Test123");
        }
    }
}
