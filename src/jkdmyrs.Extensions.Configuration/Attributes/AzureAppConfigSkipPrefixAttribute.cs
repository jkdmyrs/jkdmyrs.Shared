namespace jkdmyrs.Extensions.Configuration.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class AzureAppConfigSkipPrefixAttribute : Attribute
    {
        public AzureAppConfigSkipPrefixAttribute()
        {

        }
    }
}
