﻿using HouseRentingSystem.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseRentingSystem.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(CreateUsers());
        }

        private static List<User> CreateUsers()
        {
            var users = new List<User>();

            var hasher = new PasswordHasher<User>();

            var user = new User()
            {
                Id = "dea12856-c198-4129-b3f3-b893d8395082",
                UserName = "agent@mail.com",
                NormalizedUserName = "agent@mail.com",
                Email = "agent@mail.com",
                NormalizedEmail = "agent@mail.com",
                FirstName = "Linda",
                LastName = "Michaels",
            };

            user.PasswordHash = hasher.HashPassword(user, "agent123");
            users.Add(user);

            user = new User()
            {
                Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                UserName = "guest@mail.com",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com",
                FirstName = "Teodor",
                LastName = "Lesly"
            };

            user.PasswordHash = hasher.HashPassword(user, "guest123");
            users.Add(user);

            return users;
        }
    }
}
