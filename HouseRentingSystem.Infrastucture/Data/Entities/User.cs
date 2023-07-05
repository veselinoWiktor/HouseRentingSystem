using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Infrastructure.Data.DataConstants.User;

namespace HouseRentingSystem.Infrastructure.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; init; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; init; } = null!;
    }
}
