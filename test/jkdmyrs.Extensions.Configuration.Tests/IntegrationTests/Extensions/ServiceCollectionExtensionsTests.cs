namespace jkdmyrs.Extensions.Configuration.Tests.IntegrationTests.Extensions
{
    using FluentAssertions;
    using jkdmyrs.Extensions.Configuration.Attributes;
    using jkdmyrs.Extensions.Configuration.Extensions;
    using jkdmyrs.Testing.Common;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory(TestCategories.PipelineIntegration)]
    public class ServiceCollectionExtensionsTests
    {
        // these are real settings used in the jkdmyrs-cloud project
        // they might eventually change/be removed and cause this test to fail
        [AzureAppConfigKeyPrefix("NFL:Redirects:TableStorage")]
        private class ExampleSetting
        {
            [AzureAppConfigKey("ConnectionString")]
            // full key name: NFL:Redirects:TableStorage:ConnectionString
            public string Connection { get; set; }

            // full key name: NFL:Redirects:TableStorage:TableName
            public string TableName { get; set; }
        }

        [TestMethod]
        public void CanGetKeys()
        {
            // create a service provider
            var provider = new ServiceCollection()

                // add azure app configuration using the custom extension method AddAzureAppConfig
                .AddAzureAppConfig(TestSettings.AzureAppConfigConnectionString, TestSettings.KeyVaultCredential)

                // register the settings objects using the custom extension method AddAppConfigSetting
                .AddAppConfigSetting<ExampleSetting>(defaultPrefixToClassName: false)

                // build the provider
                .BuildServiceProvider();

            // get the settings objects from the provider
            var settings = provider.GetService<ExampleSetting>();

            // verify all the keys
            settings.Connection.Should().NotBeNull();
            settings.TableName.Should().Be("gamedirectory");
        }
    }
}
