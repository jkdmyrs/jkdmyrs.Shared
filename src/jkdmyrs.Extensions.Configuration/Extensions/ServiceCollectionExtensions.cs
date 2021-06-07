namespace jkdmyrs.Extensions.Configuration.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public static IServiceCollection AddAppConfigSetting<T>(this IServiceCollection services, char separator = ':')
            where T : class, new()
        {
            _separator = separator;
            if (!new char[] { '.', ':' }.ToList().Contains(separator))
            {
                throw new ArgumentException($"Parameter '{nameof(separator)}' in '{nameof(AddAppConfigSetting)}' was not valid.");
            }
            services.AddSingleton(sp => Instantiate<T>(sp.GetService<IAzureAppConfig>()));
            return services;
        }

        #region helpers
        private static AzureAppConfigKeyPrefixAttribute PrefixAttribute<T>() => typeof(T).GetCustomAttribute<AzureAppConfigKeyPrefixAttribute>();
        private static bool HasPrefix<T>() => PrefixAttribute<T>() != null;
        private static string Prefix<T>() => PrefixAttribute<T>().KeyPrefix;

        private static AzureAppConfigKeyPrefixAttribute PrefixAttribute(PropertyInfo property) => property.GetCustomAttribute<AzureAppConfigKeyPrefixAttribute>();
        private static AzureAppConfigSkipPrefixAttribute SkipPrefixAttribute(PropertyInfo property) => property.GetCustomAttribute<AzureAppConfigSkipPrefixAttribute>();
        private static bool HasPrefix(PropertyInfo property) => PrefixAttribute(property) != null;
        private static bool SkipPrefix(PropertyInfo property) => SkipPrefixAttribute(property) != null;
        private static string Prefix(PropertyInfo property) => PrefixAttribute(property).KeyPrefix;

        private static List<PropertyInfo> Properties<T>() => typeof(T).GetProperties().ToList();
        private static AzureAppConfigKeyAttribute KeyAttribute(PropertyInfo property) => property.GetCustomAttribute<AzureAppConfigKeyAttribute>();
        private static bool HasCustomKey(PropertyInfo property) => KeyAttribute(property) != null;
        private static string KeyName(PropertyInfo property) => HasCustomKey(property) ? KeyAttribute(property).KeyName : property.Name;

        private static string FullKeyName<T>(PropertyInfo property, bool classHasPrefix, bool propertyHasPrefix)
        {
            string keyName = string.Empty;
            if (classHasPrefix && !SkipPrefix(property))
            {
                keyName += $"{Prefix<T>()}{_separator}";
            }
            if (propertyHasPrefix)
            {
                keyName += $"{Prefix(property)}{_separator}";
            }
            keyName += KeyName(property);
            return keyName;
        }

        private static char _separator;

        private static T Instantiate<T>(IAzureAppConfig azureAppConfig) where T : class, new()
        {
            T impl = new T();
            Properties<T>().ForEach(property => property.SetValue(impl, azureAppConfig.Configuration[FullKeyName<T>(property, HasPrefix<T>(), HasPrefix(property))]));
            return impl;
        }
        #endregion helpers
    }
}