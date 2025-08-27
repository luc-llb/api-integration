using DotNetEnv;
using System.IO;

namespace IntegrationApi.Infrastructure.Data
{
    /// <summary>
    /// Provides database connection strings.
    /// </summary>
    public static class ConnectionStringProvider
    {
        private static void LoadEnvFile()
        {
            // Lista de possíveis caminhos para o arquivo .env
            var possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), ".env"),                   // Na pasta atual
                Path.Combine(Directory.GetCurrentDirectory(), "../.env"),                // Um nível acima
                Path.Combine(Directory.GetCurrentDirectory(), "../../.env"),             // Dois níveis acima
                Path.Combine(Directory.GetCurrentDirectory(), "../../../.env"),          // Três níveis acima
                Path.Combine(Directory.GetCurrentDirectory(), "../../../../.env"),        // Quatro níveis acima
                Path.Combine(AppContext.BaseDirectory, ".env"),                          // Na pasta do executável
                Path.Combine(AppContext.BaseDirectory, "../.env"),                       // Um nível acima do executável
                Path.Combine(AppContext.BaseDirectory, "../../.env"),                    // Dois níveis acima do executável
                Path.Combine(AppContext.BaseDirectory, "../../../.env"),                 // Três níveis acima do executável
                Path.Combine(AppContext.BaseDirectory, "../../../../.env"),              // Quatro níveis acima do executável
                Path.Combine(AppContext.BaseDirectory, "../../../../../.env"),           // Cinco níveis acima do executável
            };

            // Procura o arquivo .env em todos os caminhos possíveis
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    // Console.WriteLine($"Load .env file from: {path}");
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
