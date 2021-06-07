namespace jkdmyrs.Extensions.Configuration.Tests
{
    using System.Reflection;
    using Microsoft.Extensions.Configuration;

    public static class TestSettings
    {
        private static IConfiguration _config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();
        public static string ConnectionString = _config["jkdmyrsconfigconnection"];
        public static string TenantId = _config["jkdmyrsconfigtenant"];
        public static string ClientId = _config["jkdmyrsconfigid"];
        public static string ClientSecret = _config["jkdmyrsconfigsecret"];
    }
}
