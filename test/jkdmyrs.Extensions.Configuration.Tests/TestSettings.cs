namespace jkdmyrs.Extensions.Configuration.Tests
{
    using System;
    using System.Reflection;
    using Azure.Core;
    using Azure.Identity;
    using Microsoft.Extensions.Configuration;

    public static class TestSettings
    {
        private static IConfiguration _config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        private static string ConnectionString = _config["jkdmyrsconfigconnection"];
        private static string TenantId = _config["jkdmyrsconfigtenant"];
        private static string ClientId = _config["jkdmyrsconfigid"];
        private static string ClientSecret = _config["jkdmyrsconfigsecret"];

        public static TokenCredential KeyVaultCredential
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TenantId) || string.IsNullOrWhiteSpace(ClientId) || string.IsNullOrWhiteSpace(ClientSecret))
                {
                    return new DefaultAzureCredential();
                }
                else
                {
                    return new ClientSecretCredential(TenantId, ClientId, ClientSecret);
                }
            }
        }

        public static string AzureAppConfigConnectionString
        {
            get
            {
                string envVarName = "APP_CONFIG_CONNECTION";
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    return ConnectionString;
                }
                // try to get the connection string from the process first
                string connectionString = Environment.GetEnvironmentVariable(envVarName, EnvironmentVariableTarget.Process);

                // if it is not found in the process target, check the user target
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    connectionString = Environment.GetEnvironmentVariable(envVarName, EnvironmentVariableTarget.User);
                }

                // if we haven't found it by now, return empty string
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return string.Empty;
                }

                return connectionString;
            }
        }
    }
}
