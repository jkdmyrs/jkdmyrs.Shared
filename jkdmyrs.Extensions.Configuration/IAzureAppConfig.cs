namespace jkdmyrs.Extensions.Configuration
{
    using Microsoft.Extensions.Configuration;

    public interface IAzureAppConfig
    {
        IConfiguration Configuration { get; }
    }
}
