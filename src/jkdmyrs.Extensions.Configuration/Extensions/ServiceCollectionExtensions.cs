namespace jkdmyrs.Extensions.Configuration.Extensions
{
    using System;
    using System.Linq;
    using Azure.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureAppConfiguration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureAppConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddAzureAppConfiguration();
            services.AddSingleton<IAzureAppConfig>(new AzureAppConfig(config));
            return services;
        }

        public static IServiceCollection AddAzureAppConfig(this IServiceCollection services, string connectionString, TokenCredential keyvaultCredentials = null, Action<AzureAppConfigurationOptions> configureOptions = null)
        {
            services.AddAzureAppConfiguration();
            services.AddSingleton<IAzureAppConfig>(sp => {
              var config = new ConfigurationBuilder()
                .AddAzureAppConfig(connectionString, keyvaultCredentials, configureOptions)
                .Build();
              return new AzureAppConfig(config);
            });
            return services;
        }

        public static IServiceCollection AddAppConfigSetting<T>(this IServiceCollection services, char separator = ':', bool defaultPrefixToClassName = true)
            where T : class, new()
        {
            if (!new char[] { '.', ':' }.ToList().Contains(separator))
            {
                throw new ArgumentException($"Parameter '{nameof(separator)}' in '{nameof(AddAppConfigSetting)}' was not valid.");
            }
            services.AddSingleton(sp => Instantiate<T>(sp.GetService<IAzureAppConfig>(), defaultPrefixToClassName, separator));
            return services;
        }

        #region helpers
        private static T Instantiate<T>(IAzureAppConfig azureAppConfig, bool useClassNameAsPrefix, char separator) where T : class, new()
        {
            T impl = new T();
            typeof(T).Properties().ForEach(property => property.SetValue(impl, azureAppConfig.Configuration[property.FullKeyName<T>(useClassNameAsPrefix, separator)]));
            return impl;
        }
        #endregion helpers
    }
}