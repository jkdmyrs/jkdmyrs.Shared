namespace jkdmyrs.Extensions.Configuration.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using jkdmyrs.Extensions.Configuration.Attributes;

    public static class TypeExtensions
    {
        private static AzureAppConfigKeyPrefixAttribute PrefixAttribute(this Type type) => type.GetCustomAttribute<AzureAppConfigKeyPrefixAttribute>();
        public static bool HasPrefix(this Type type, bool classNameAsPrefix) => type.PrefixAttribute() != null || classNameAsPrefix;
        public static string Prefix(this Type type) => type.PrefixAttribute() != null ? type.PrefixAttribute().KeyPrefix : type.Name;
        public static List<PropertyInfo> Properties(this Type type) => type.GetProperties().ToList();
    }
}
