using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Infrastucture.Data.DataConstants.Agent;

namespace HouseRentingSystem.Models.Agent
{
    public class BecomeAgentFormModel
    {
        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; init; } = null!;
    }
}
