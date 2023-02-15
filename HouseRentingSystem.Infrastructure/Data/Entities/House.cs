using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using static HouseRentingSystem.Infrastructure.Data.DataConstants.House;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Data.Entities
{
    public class House
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(MaxAddressLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Range(MinPriceValue,MaxPriceValue)]
        public decimal PricePerMonth { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        [Required]
        public int AgentId { get; set; }

        public Agent Agent { get; set; } = null!;

        public string? RenterId { get; set; }
    }
}
//•	Id – a unique integer, Primary Key
//•	Title – a string with min length 10 and max length 50 (required)
//•	Address – a string with min length 30 and max length 150 (required)
//•	Description – a string with min length 50 and max length 500 (required)
//•	ImageUrl – a string(required)
//•	PricePerMonth – a decimal with min value 0 and max value 2000 (required)
//•	CategoryId – an integer(required)
//•	Category – a Category object
//•	AgentId – an integer (required)
//•	Agent – an Agent object
//•	RenterId – a string
