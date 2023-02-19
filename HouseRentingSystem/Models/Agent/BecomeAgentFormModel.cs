using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Infrastructure.Data.DataConstants.Agent;

namespace HouseRentingSystem.Models.Agent
{
    public class BecomeAgentFormModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [StringLength(MaxPhoneNumberLength, MinimumLength = MinPhoneNumberLength)]
        public string PhoneNumber { get; init; } = null!;
    }
}
