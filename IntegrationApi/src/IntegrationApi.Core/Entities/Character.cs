using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace IntegrationApi.Core.Entities
{
    /// <summary>
    /// Represents a character in an animated series.
    /// </summary>
    public class Character : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [AllowNull]
        public string Gender { get; set; }
        [AllowNull]
        public DateTime DateOfBirth { get; set; }
        [AllowNull]
        public string[] AlternativeName { get; set; }
        [AllowNull]
        [ForeignKey(nameof(Animation))]
        public int AnimationId { get; set; }

        public Character(int id, string name, string gender, DateTime dateOfBirth, string[] alternativeName, int animationId)
        {
            Id = id;
            Name = name;
            Gender = gender ?? "Unknown";
            DateOfBirth = dateOfBirth;
            AlternativeName = alternativeName ?? Array.Empty<string>();
            AnimationId = animationId;
        }

    }
}