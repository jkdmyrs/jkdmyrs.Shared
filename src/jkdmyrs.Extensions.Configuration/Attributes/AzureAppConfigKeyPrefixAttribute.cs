namespace jkdmyrs.Extensions.Configuration.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class AzureAppConfigKeyPrefixAttribute : Attribute
    {
        public string KeyPrefix { get; private set; }

        public AzureAppConfigKeyPrefixAttribute(string keyPrefix)
        {
            if (string.IsNullOrWhiteSpace(keyPrefix))
            {
                throw new ArgumentException($"Parameter '{nameof(keyPrefix)}' in '{nameof(AzureAppConfigKeyPrefixAttribute)}' cannot be null or whitespace.");
            }
            KeyPrefix = keyPrefix;
        }
    }
}
