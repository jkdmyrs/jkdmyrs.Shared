namespace jkdmyrs.Extensions.Configuration.Tests.IntegrationTests.Extensions
{
    using Azure.Identity;
    using FluentAssertions;
    using jkdmyrs.Extensions.Configuration.Attributes;
    using jkdmyrs.Extensions.Configuration.Extensions;
    using jkdmyrs.Testing.Common;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory(TestCategories.Integration)]
    public class ServiceCollectionExtensionsTests
    {
        private class ExampleSetting
        {
            // full key name: TestConfigKey
            public string TestConfigKey { get; set; }

            [AzureAppConfigKey("TestSettings:ExampleKey")]
            // full key name: TestSettings:ExampleKey
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
            // create a service provider
            var provider = new ServiceCollection()

                // add azure app configuration using the custom extension method AddAzureAppConfig
                .AddAzureAppConfig(TestSettings.ConnectionString, new ClientSecretCredential(TestSettings.TenantId, TestSettings.ClientId, TestSettings.ClientSecret))

                // register the settings objects using the custom extension method AddAppConfigSetting
                .AddAppConfigSetting<ExampleSetting>(defaultPrefixToClassName: false)
                .AddAppConfigSetting<ExampleSetting2>()

                // build the provider
                .BuildServiceProvider();

            // get the settings objects from the provider
            var settings = provider.GetService<ExampleSetting>();
            var settings2 = provider.GetService<ExampleSetting2>();

            // verify all the keys
            settings.TestConfigKey.Should().Be("TestConfigValue");
            settings.TestSecret.Should().Be("TestConfigSecretValue");
            settings2.Version.Should().Be("123");
            settings2.AppName.Should().Be("test");
            settings2.ClientKey.Should().Be("TestConfigValue");
            settings2.AuthKey.Should().Be("Test123");
        }
    }
}
