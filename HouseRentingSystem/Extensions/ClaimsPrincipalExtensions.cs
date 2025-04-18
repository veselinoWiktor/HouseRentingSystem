﻿using System.Security.Claims;
using static HouseRentingSystem.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAdmin(this ClaimsPrincipal user) 
        {
            return user.IsInRole(AdminRoleName);
        }
    }
}
