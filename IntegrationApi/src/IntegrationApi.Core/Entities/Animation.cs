using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IntegrationApi.Core.Entities
{
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
        public DateTime EndDate { get; set; }
        [AllowNull]
        public string[] AlternativeTitle { get; set; }

        public Animation(int id, string title, int episodes, int duration, string[] genres, string season, DateTime endDate, string[] alternativeTitle)
        {
            Id = id;
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