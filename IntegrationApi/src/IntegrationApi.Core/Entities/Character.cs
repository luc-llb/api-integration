using System.ComponentModel.DataAnnotations;

namespace IntegrationApi.Core.Entities
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}