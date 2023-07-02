using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Infrastucture.Data.DataConstants.Category;

namespace HouseRentingSystem.Infrastucture.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
       
        public IEnumerable<House> Houses { get; init; }
            = new List<House>();
    }
}
