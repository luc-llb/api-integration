using System.Net.Http;
using System.Text;
using System.Text.Json;
using IntegrationApi.Core.DTOs;
using System.Threading.Tasks;

namespace IntegrationApi.Infrastructure.Services{

    /// <summary>
    /// Service for interacting with GraphQL APIs.
    /// </summary>
    public class GraphQLService{
        private readonly HttpClient _httpClient;

        public GraphQLService(HttpClient httpClient){
            _httpClient = httpClient;
        }

        /// <summary>
        /// Creates the content for a GraphQL request.
        /// </summary>
        /// <param name="query">The GraphQL query string.</param>
        /// <param name="variables">Optional variables for the GraphQL query.</param>
        /// <returns>A StringContent object containing the serialized GraphQL request.</returns>
        public StringContent CreateGraphQLContent(string query, QueryVariables? variables = null){
            var requestBody = new {
                query,
                variables
            };

            return new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );
        }

        /// <summary>
        /// Executes a GraphQL query against the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The GraphQL endpoint URL.</param>
        /// <param name="content">The GraphQL request content.</param>
        /// <returns>The response from the GraphQL server as a string.</returns>
        public async Task<string> ExecuteQueryAsync(string endpoint, StringContent content){
            var requestBody = await content.ReadAsStringAsync();
            
            var response = await _httpClient.PostAsync(endpoint, new StringContent(
                requestBody,
                Encoding.UTF8,
                "application/json"
            ));

            var responseContent = await response.Content.ReadAsStringAsync();
            
            try{
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException ex){
                throw new ApplicationException(
                    $"Error executing a GraphQL query.\n" +
                    $"URL: {endpoint}\n" +
                    $"Request: {requestBody}\n" +
                    $"Response: {responseContent}", 
                    ex
                );
            }
            
            return responseContent;
        }
    }
}