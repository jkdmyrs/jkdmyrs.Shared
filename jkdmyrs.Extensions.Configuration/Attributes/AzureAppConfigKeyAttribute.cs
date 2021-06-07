namespace jkdmyrs.Extensions.Configuration.Attributes
{
    using System;

    public class AzureAppConfigKeyAttribute : Attribute
    {
        public string KeyName { get; private set; }

        public AzureAppConfigKeyAttribute(string keyName)
        {
            KeyName = keyName;
        }
    }
}
