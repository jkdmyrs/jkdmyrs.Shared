namespace jkdmyrs.Extensions.Configuration.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class AzureAppConfigKeyAttribute : Attribute
    {
        public string KeyName { get; private set; }

        public AzureAppConfigKeyAttribute(string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException($"Parameter '{nameof(keyName)}' in '{nameof(AzureAppConfigKeyAttribute)}' cannot be null or whitespace.");
            }
            KeyName = keyName;
        }
    }
}
