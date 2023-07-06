using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Data.Migrations
{
    public partial class AddedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f62b789b-ada7-440f-ab9c-42294d8a163f", "AQAAAAEAACcQAAAAEGKiQRlV49vBEu9kUzR+v2a6LKRxenLO0FFr3olM+hqeYYWt37RFACKBkyz1ilyFHA==", "0406cc44-7dfe-4b13-aa0c-f6eb95da22a8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0e40aa3-6b7e-4ded-904a-9963507d37ed", "AQAAAAEAACcQAAAAEP4BkbcBnFbBo885n/9edtG0hRDaVSJ5ojf0mUjBhBuI23fsTYJaqOdFsM7YqPgoSA==", "4fdf93d6-e9c0-4303-9890-063dbba107d3" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bcb4f072-ecca-43c9-ab26-c060c6f364e4", 0, "61cccd40-6836-4e57-b7e2-cfca6c7f6a65", "admin@mail.com", false, "Great", "Admin", false, null, "admin@mail.com", "admin@mail.com", "AQAAAAEAACcQAAAAEMbJhOdgdOI/YJo8ft+Y0L40NbiIn8onfxMiBY6Z0gP43n+B20nwBRbnu8vmzgRrCQ==", null, false, "0ce7d0f9-c845-4a8d-b77b-48145188490d", false, "admin@mail.com" });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "PhoneNumber", "UserId" },
                values: new object[] { 5, "+359123456789", "bcb4f072-ecca-43c9-ab26-c060c6f364e4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d446c181-4c2d-448f-96f9-921658a6b4a3", "AQAAAAEAACcQAAAAEMx2Do4eAaACQ+dXrAJ/x3a+5SAyOkxZnMUE1M9QlBHdQaKjAOPvT8kQrn1BJ/iuyg==", "e08eb8f3-c07a-44b0-a40f-38f77aa9fd5e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bfc89bf9-8f34-4982-a684-d8c0b348d4f6", "AQAAAAEAACcQAAAAEP3qwNOTeE5kS45WxQXBAe6ZNxHp+N8an5Q5rTAEs5k1Q1sybJ+GigTL5oErb9IgxA==", "0bc0d562-ee6d-460c-943b-82cdce348749" });
        }
    }
}
