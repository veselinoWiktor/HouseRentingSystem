using HouseRentingSystem.Core.Models.House;
using System.Text.RegularExpressions;

namespace HouseRentingSystem.Extensions
{
    public static class ModelExtensions
    {
        public static string GetInformation(this IHouseModel house)
        {
            return house.Title.Replace(" ", "-") + "-" + GetAddress(house.Address);
        }

        private static string GetAddress(string address)
        {
            address = String.Join("-", address.Split(" ").Take(3));
            return Regex.Replace(address, @"[^a-zA-Z0-9\-]", String.Empty);
        }
    }
}
