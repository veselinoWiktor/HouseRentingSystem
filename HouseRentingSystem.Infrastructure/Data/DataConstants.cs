using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Data
{
    public static class DataConstants
    {
        public static class Category 
        {
            public const int MaxNameLength = 50;
        }

        public static class House 
        {
            public const int MaxTitleLength = 50;
            public const int MinTitleLength = 10;

            public const int MaxAddressLength = 150;
            public const int MinAddressLength = 30;

            public const int MaxDescriptionLength = 500;
            public const int MinDescriptionLength = 50;

            public const int MaxPriceValue = 200;
            public const int MinPriceValue = 0;
        }

        public static class Agent
        {
            public const int MaxPhoneNumberLength = 15;
            public const int MinPhoneNumberLength = 7;
        }
    }
}
//•	Id – a unique integer, Primary Key
//•	PhoneNumber – a string with min length 7 and max length 15 (required)
//•	UserId – a string(required)
//•	User – an IdentityUser object

