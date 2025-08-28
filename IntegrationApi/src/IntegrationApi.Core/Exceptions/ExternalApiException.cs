namespace IntegrationApi.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs while interacting with an external API.
    /// </summary>
    public class ExternalApiException : Exception
    {
        public string ApiName { get; }
        public string Endpoint { get; }

        public ExternalApiException(string apiName, string endpoint, string message) 
            : base($"Error in external API {apiName} at endpoint {endpoint}: {message}")
        {
            ApiName = apiName;
            Endpoint = endpoint;
        }

        public ExternalApiException(string apiName, string endpoint, string message, Exception innerException) 
            : base($"Error in external API {apiName} at endpoint {endpoint}: {message}", innerException)
        {
            ApiName = apiName;
            Endpoint = endpoint;
        }
    }
}
