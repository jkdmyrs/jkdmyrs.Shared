namespace jkdmyrs.Extensions.Configuration.Extensions
{
    using System.Reflection;
    using jkdmyrs.Extensions.Configuration.Attributes;

    public static class PropertyInfoExtensions
    {
        private static AzureAppConfigKeyPrefixAttribute PrefixAttribute(this PropertyInfo property) => property.GetCustomAttribute<AzureAppConfigKeyPrefixAttribute>();
        private static AzureAppConfigSkipPrefixAttribute SkipPrefixAttribute(this PropertyInfo property) => property.GetCustomAttribute<AzureAppConfigSkipPrefixAttribute>();
        private static bool HasPrefix(this PropertyInfo property) => PrefixAttribute(property) != null;
        private static bool SkipPrefix(this PropertyInfo property) => SkipPrefixAttribute(property) != null;
        private static string Prefix(this PropertyInfo property) => PrefixAttribute(property).KeyPrefix;
        private static AzureAppConfigKeyAttribute KeyAttribute(this PropertyInfo property) => property.GetCustomAttribute<AzureAppConfigKeyAttribute>();
        private static bool HasCustomKey(this PropertyInfo property) => KeyAttribute(property) != null;
        private static string KeyName(this PropertyInfo property) => HasCustomKey(property) ? KeyAttribute(property).KeyName : property.Name;

        public static string FullKeyName<T>(this PropertyInfo property, bool classNameAsPrefix, char separator)
        {
            string keyName = string.Empty;
            if (typeof(T).HasPrefix(classNameAsPrefix) && !property.SkipPrefix())
            {
                keyName += $"{typeof(T).Prefix()}{separator}";
            }
            if (property.HasPrefix())
            {
                keyName += $"{property.Prefix()}{separator}";
            }
            keyName += property.KeyName();
            return keyName;
        }
    }
}
