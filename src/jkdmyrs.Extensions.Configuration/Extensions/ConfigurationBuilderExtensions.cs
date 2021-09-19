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
                // connect
                o.Connect(connectionString);

                // connect to keyvault if credentials provided
                if (keyvaultCredentials != null)
                {
                    o.ConfigureKeyVault(kv => {
                        kv.SetCredential(keyvaultCredentials);
                    });
                }

                // if a custom config action was provided, run it
                if (configureOptions != null)
                {
                    configureOptions(o);
                }
                // otherwise, just select all the keys
                else
                {
                    o.Select(KeyFilter.Any, LabelFilter.Null);
                }
            });
            return builder;
        }
    }
}
