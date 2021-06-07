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

        [TestMethod]
        public void CanGetKeys()
        {
            // setup a service provider
            var provider = new ServiceCollection()
                .AddAzureAppConfig(TestSettings.ConnectionString, new ClientSecretCredential(TestSettings.TenantId, TestSettings.ClientId, TestSettings.ClientSecret))
                .AddAppConfigSetting<ExampleSetting>()
                .BuildServiceProvider();

            // get the settings from the provider
            var settings = provider.GetService<ExampleSetting>();

            // verify the key without AzureAppConfigKey attribute has value
            settings.TestConfigKey.Should().Be("TestConfigValue");
            // verify the key with AzureAppConfigKey attribute has value
            settings.TestSecret.Should().Be("TestConfigSecretValue");
        }
    }
}
