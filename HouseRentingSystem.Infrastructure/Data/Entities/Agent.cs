﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Infrastructure.Data.DataConstants.Agent;

namespace HouseRentingSystem.Infrastructure.Data.Entities
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        public IdentityUser User { get; set; } = null!;
    }
}
//•	Id – a unique integer, Primary Key
//•	PhoneNumber – a string with min length 7 and max length 15 (required)
//•	UserId – a string(required)
//•	User – an IdentityUser object

