using System.ComponentModel.DataAnnotations;

namespace IntegrationApi.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int AniListId { get; set; }
    }
}