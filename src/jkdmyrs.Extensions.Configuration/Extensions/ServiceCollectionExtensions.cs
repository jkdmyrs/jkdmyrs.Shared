namespace jkdmyrs.Extensions.Configuration.Extensions
{
    using System;
    using System.Reflection;
    using Azure.Core;
    using jkdmyrs.Extensions.Configuration.Attributes;
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
            services.AddSingleton<IAzureAppConfig>(new ConfigurationBuilder()
                .AddAzureAppConfig(connectionString, keyvaultCredentials, configureOptions)
                .BuildAzureAppConfig());
            return services;
        }

        public static IServiceCollection AddAppConfigSetting<T>(this IServiceCollection services)
            where T : class, new()
        {
            services.AddSingleton<T>(sp =>
            {
                var config = sp.GetService<IAzureAppConfig>();
                T implementation = new T();
                var properties = typeof(T).GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    string propertyName = prop.Name;
                    var keyNameAttribute = prop.GetCustomAttribute<AzureAppConfigKeyAttribute>();

                    string keyName = keyNameAttribute == null ? propertyName : keyNameAttribute.KeyName;
                    prop.SetValue(implementation, config.Configuration[keyName]);
    
                }
                return implementation;
            });
            return services;
        }
    }
}