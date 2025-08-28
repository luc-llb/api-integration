namespace IntegrationApi.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a validation error occurs.
    /// </summary>
    public class ValidationException : Exception
    {
        public string? PropertyName { get; }

        public ValidationException(string propertyName, string message) 
            : base($"Error in property validation {propertyName}: {message}")
        {
            PropertyName = propertyName;
        }

        public ValidationException(string message) : base(message)
        {
            PropertyName = null;
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            PropertyName = null;
        }
    }
}
