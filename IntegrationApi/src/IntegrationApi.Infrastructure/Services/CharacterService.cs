using IntegrationApi.Core.Entities;
using IntegrationApi.Core.Interfaces;
using IntegrationApi.Core.DTOs;
using IntegrationApi.Infrastructure.Data;
using System.Text.Json;

namespace IntegrationApi.Infrastructure.Services
{
    public class CharacterService : RepositoryBase<Character>, ICharacterService
    {
        private readonly GraphQLService _graphQLService;
        private readonly string _graphQLEndpoint = "https://graphql.anilist.co";
        private const string _query = @"
            query Character($search: String, $type: MediaType) {
                Character(search: $search) {
                    id
                    gender
                    name {
                        full
                        alternative
                    }
                    dateOfBirth {
                        day
                        month
                        year
                    }
                    media(type: $type) {
                        nodes {
                            id
                        }
                    }
                }
            }";

        public CharacterService(AppDbContext context, GraphQLService graphQLService) : base(context)
        {
            _graphQLService = graphQLService ?? throw new ArgumentNullException(nameof(graphQLService));
        }

        public async Task<IEnumerable<Character>> SearchCharactersAsync(string searchTerm, MediaType mediaType)
        {
            var variables = new QueryVariables
            {
                Search = searchTerm,
                Type = mediaType.ToString()
            };
            
            var content = _graphQLService.CreateGraphQLContent(_query, variables);
            
            var result = await _graphQLService.ExecuteQueryAsync(_graphQLEndpoint, content);

            return ParseCharactersFromGraphQLResponse(result);
        }

        private IEnumerable<Character> ParseCharactersFromGraphQLResponse(string response)
        {
            var characters = new List<Character>();

            try
            {
                var root = JsonDocument.Parse(response).RootElement;

                if (root.TryGetProperty("data", out var data) &&
                data.TryGetProperty("Character", out var characterData))
                {
                    Character character = new();

                    if (characterData.TryGetProperty("id", out var externalId))
                        character.AniListId = externalId.GetInt32();

                    if (characterData.TryGetProperty("name", out var name) && name.TryGetProperty("full", out var fullName))
                    {
                        character.Name = fullName.GetString() ?? "Unknown";
                        character.AlternativeName = GetAlternativeName(name);
                    }
                        
                    if (characterData.TryGetProperty("gender", out var gender))
                        character.Gender = gender.GetString() ?? "Unknown";

                    if (characterData.TryGetProperty("dateOfBirth", out var dateOfBirth))
                        character.DateOfBirth = GetDateOfBirth(dateOfBirth);

                    if (characterData.TryGetProperty("media", out var media) &&
                        media.TryGetProperty("nodes", out var nodes) &&
                        nodes.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var mediaNode in nodes.EnumerateArray())
                        {
                            if (mediaNode.TryGetProperty("id", out var
                                animationId))
                            {
                                character.AnimationId = animationId.GetInt32();
                                break; // Assuming we only want the first related animation
                            }
                        }
                    }
                    characters.Add(character);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing GraphQL response: {ex.Message}");
            }

            return characters;
        }

        private DateTime? GetDateOfBirth(JsonElement date)
        {
            int? day = date.TryGetProperty("day", out var dayElement) && dayElement.ValueKind != JsonValueKind.Null ? dayElement.GetInt32() : null;
            int? month = date.TryGetProperty("month", out var monthElement) && monthElement.ValueKind != JsonValueKind.Null ? monthElement.GetInt32() : null;
            int? year = date.TryGetProperty("year", out var yearElement) && yearElement.ValueKind != JsonValueKind.Null ? yearElement.GetInt32() : null;

            if (year.HasValue && month.HasValue && day.HasValue)
            return new DateTime(year.Value, month.Value, day.Value);

            return null;
        }

        private string[] GetAlternativeName(JsonElement name)
        {
            if (name.TryGetProperty("alternative", out var alternative) &&
                alternative.ValueKind == JsonValueKind.Array)
            {
                var names = new List<string>();
                foreach (var item in alternative.EnumerateArray())
                {
                    if (item.ValueKind != JsonValueKind.Null)
                        names.Add(item.GetString() ?? string.Empty);
                }
                return names.ToArray();
            }
            return Array.Empty<string>();
        }
    }
}
