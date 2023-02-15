using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Infrastructure.Data.DataConstants.Category;

namespace HouseRentingSystem.Infrastructure.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(MaxNameLength)]    
        public string Name { get; set; } = null!;

        public IEnumerable<House> Houses { get; init; }
            = new List<House>();
    }
}
//•	Id – a unique integer, Primary Key
//•	Name – a string with max length 50 (required)
//•	Houses – a collection of House
