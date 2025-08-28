using System;

namespace IntegrationApi.Core.Exceptions
{
    /// <summary>
    /// Exceção lançada quando um recurso não é encontrado
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base("The resource was not found.")
        {
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
