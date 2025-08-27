using DotNetEnv;

namespace IntegrationApi.Infrastructure.Data
{
    /// <summary>
    /// Provides database connection strings.
    /// </summary>
    public static class ConnectionStringProvider
    {
        /// <summary>
        /// Loads the .env file from the possible paths.
        /// </summary>
        private static void LoadEnvFile()
        {
            var possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), ".env"),
                Path.Combine(Directory.GetCurrentDirectory(), "../.env"),
                Path.Combine(Directory.GetCurrentDirectory(), "../../.env"),
                Path.Combine(Directory.GetCurrentDirectory(), "../../../.env"),
                Path.Combine(Directory.GetCurrentDirectory(), "../../../../.env"),
                Path.Combine(AppContext.BaseDirectory, ".env"),
                Path.Combine(AppContext.BaseDirectory, "../.env"),
                Path.Combine(AppContext.BaseDirectory, "../../.env"),
                Path.Combine(AppContext.BaseDirectory, "../../../.env"),
                Path.Combine(AppContext.BaseDirectory, "../../../../.env"),
                Path.Combine(AppContext.BaseDirectory, "../../../../../.env"),
            };

            // Search for the .env file in all possible paths
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    Env.Load(path);
                    return;
                }
            }

            throw new FileNotFoundException(".env file not found in any of the expected locations.");
        }

        public static string GetConnectionString()
        {
            LoadEnvFile();
            var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var trustCert = Environment.GetEnvironmentVariable("DB_TRUST_CERTIFICATE") ?? "True";

            if (string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(dbName))
            {
                throw new InvalidOperationException($"One or more database environment variables are not set in the .env file.\n" +
                $"{nameof(dbType)}: {dbType}, {nameof(host)}: {host}, {nameof(port)}: {port}, {nameof(user)}: {user}, {nameof(password)}: {(string.IsNullOrEmpty(password) ? "null or empty" : "set")}, {nameof(dbName)}: {dbName}");
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
