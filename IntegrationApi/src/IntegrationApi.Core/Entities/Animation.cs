using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IntegrationApi.Core.Entities
{
    /// <summary>
    /// Represents an animated series.
    /// </summary>
    public class Animation : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        
        [AllowNull]
        public int Episodes { get; set; }
        
        [AllowNull]
        public int Duration { get; set; }
        
        [AllowNull]
        public string[] Genres { get; set; }
        
        [AllowNull]
        public string Season { get; set; }
        
        [AllowNull]
        public DateTime? EndDate { get; set; }
        
        [AllowNull]
        public string[] AlternativeTitle { get; set; }

        public Animation()
        {
            Title = string.Empty;
            Genres = Array.Empty<string>();
            Season = "Unknown";
            AlternativeTitle = Array.Empty<string>();
        }

        public Animation(int aniListId, string title, int episodes, int duration, string[] genres, string season, DateTime? endDate, string[] alternativeTitle)
        {
            AniListId = aniListId;
            Title = title;
            Episodes = episodes;
            Duration = duration;
            Genres = genres ?? Array.Empty<string>();
            Season = season ?? "Unknown";
            EndDate = endDate;
            AlternativeTitle = alternativeTitle ?? Array.Empty<string>();
        }
    }
}