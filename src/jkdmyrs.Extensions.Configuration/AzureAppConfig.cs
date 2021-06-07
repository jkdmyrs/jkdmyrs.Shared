namespace jkdmyrs.Extensions.Configuration
{
    using Microsoft.Extensions.Configuration;

    public class AzureAppConfig : IAzureAppConfig
    {
        public IConfiguration Configuration { get; private set; }

        public AzureAppConfig(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
