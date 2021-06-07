using System;

namespace jkdmyrs.Extensions.Configuration.Tests
{
    public static class TestSettings
    {
        public static string ConnectionString = GetEnvVar("jkdmyrsconfigconnection");
        public static string TenantId = GetEnvVar("jkdmyrsconfigtenant");
        public static string ClientId = GetEnvVar("jkdmyrsconfigid");
        public static string ClientSecret = GetEnvVar("jkdmyrsconfigsecret");

        private static string GetEnvVar(string envVarName)
        {
            // try to get the env var from the process first
            string envVar = Environment.GetEnvironmentVariable(envVarName, EnvironmentVariableTarget.Process);

            // if it is not found in the process target, check the user target
            if (string.IsNullOrWhiteSpace(envVar))
            {
                envVar = Environment.GetEnvironmentVariable(envVarName, EnvironmentVariableTarget.User);
            }

            // if we haven't found it by now, return empty string
            if (string.IsNullOrWhiteSpace(envVar))
            {
                return string.Empty;
            }

            return envVar;
        }
    }
}
