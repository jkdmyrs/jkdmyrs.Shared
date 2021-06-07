namespace jkdmyrs.Extensions.Configuration.Extensions
{
    using System;
    using Azure.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureAppConfiguration;

    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAzureAppConfig(this IConfigurationBuilder builder, string connectionString, TokenCredential keyvaultCredentials = null, Action<AzureAppConfigurationOptions> configureOptions = null)
        {
            builder.AddAzureAppConfiguration(o => {
                o.Connect(connectionString)
                    .Select(KeyFilter.Any, LabelFilter.Null);
                if (keyvaultCredentials != null)
                {
                    o.ConfigureKeyVault(kv => {
                        kv.SetCredential(keyvaultCredentials);
                    });
                }
                if (configureOptions != null)
                {
                    configureOptions(o);
                }
            });
            return builder;
        }

        public static IAzureAppConfig BuildAzureAppConfig(this IConfigurationBuilder builder)
        {
            return new AzureAppConfig(builder.Build());
        }
    }
}
