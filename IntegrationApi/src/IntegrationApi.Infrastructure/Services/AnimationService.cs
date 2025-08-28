using IntegrationApi.Core.Entities;
using IntegrationApi.Core.Interfaces;
using IntegrationApi.Core.DTOs;
using IntegrationApi.Infrastructure.Data;
using System.Text.Json;

namespace IntegrationApi.Infrastructure.Services
{
    public class AnimationService : RepositoryBase<Animation>, IAnimationService
    {
        private readonly GraphQLService _graphQLService;
        private readonly string _graphQLEndpoint = "https://graphql.anilist.co";
        private const string _query = @"
            query Media($search: String, $type: MediaType) {
                Media(search: $search, type: $type) {
                    id
                    title {
                        english
                        romaji
                    }
                    episodes
                    duration
                    genres
                    season
                    endDate {
                        day
                        month
                        year
                    }
                }
            }";

        public AnimationService(AppDbContext context, GraphQLService graphQLService) : base(context)
        {
            _graphQLService = graphQLService ?? throw new ArgumentNullException(nameof(graphQLService));
        }
        
        public async Task<IEnumerable<Animation>> SearchAnimationsAsync(string searchTerm, MediaType mediaType)
        {
            var variables = new QueryVariables
            {
                Search = searchTerm,
                Type = mediaType.ToString()
            };
            
            var content = _graphQLService.CreateGraphQLContent(_query, variables);
            
            var result = await _graphQLService.ExecuteQueryAsync(_graphQLEndpoint, content);
            
            return ParseAnimationsFromGraphQLResponse(result);
        }
        
        private IEnumerable<Animation> ParseAnimationsFromGraphQLResponse(string response)
        {
            var animations = new List<Animation>();
            
            try
            {
                var jsonDocument = JsonDocument.Parse(response);
                var root = jsonDocument.RootElement;
                
                if (root.TryGetProperty("data", out var data) && 
                    data.TryGetProperty("Media", out var media))
                {
                    var animation = new Animation
                    {
                        Title = GetTitle(media)
                    };
                    
                    if (media.TryGetProperty("id", out var externalId))
                        animation.AniListId = externalId.GetInt32();
                    
                    if (media.TryGetProperty("episodes", out var episodes))
                        animation.Episodes = episodes.ValueKind != JsonValueKind.Null ? episodes.GetInt32() : 0;
                        
                    if (media.TryGetProperty("duration", out var duration))
                        animation.Duration = duration.ValueKind != JsonValueKind.Null ? duration.GetInt32() : 0;
                        
                    if (media.TryGetProperty("genres", out var genres) && genres.ValueKind == JsonValueKind.Array)
                        animation.Genres = genres.EnumerateArray()
                             .Select(g => g.GetString() ?? string.Empty).ToArray();
                    
                    animations.Add(animation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing GraphQL response: {ex.Message}");
            }
            
            return animations;
        }

        /// <summary>
        /// Get the animation's title. First in Romaji and then in English.
        /// </summary>
        /// <param name="media">JsonElement with media data</param>
        /// <returns>The title in Romaji or English, otherwise it returns an empty string</returns>
        private string GetTitle(JsonElement media)
        {
            if (media.TryGetProperty("title", out var title))
            {
                if (title.TryGetProperty("english", out var english) &&
                    english.ValueKind != JsonValueKind.Null)
                    return english.GetString() ?? string.Empty;

                if (title.TryGetProperty("romaji", out var romaji) &&
                    romaji.ValueKind != JsonValueKind.Null)
                    return romaji.GetString() ?? string.Empty;
            }

            return string.Empty;
        }
    }
}