using DotNetEnv;

namespace IntegrationApi.Infrastructure.Data
{
    /// <summary>
    /// Provides database connection strings.
    /// </summary>
    public static class ConnectionStringProvider
    {
        public static string GetConnectionString()
        {
            Env.Load();
            var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var trustCert = Environment.GetEnvironmentVariable("DB_TRUST_CERTIFICATE") ?? "True";

            if (string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(dbName))
            {
                throw new InvalidOperationException("One or more database environment variables are not set in the .env file.");
            }

            if (dbType == "sqlserver")
            {
                return $"Server={host},{port};Database={dbName};User Id={user};Password={password};TrustServerCertificate={trustCert};";
            }
            else
            {
                return $"Host={host};Port={port};Username={user};Password={password};Database={dbName}";
            }
        }
    }
}
