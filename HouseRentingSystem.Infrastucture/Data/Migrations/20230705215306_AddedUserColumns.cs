using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Data.Migrations
{
    public partial class AddedUserColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d446c181-4c2d-448f-96f9-921658a6b4a3", "Teodor", "Lesly", "AQAAAAEAACcQAAAAEMx2Do4eAaACQ+dXrAJ/x3a+5SAyOkxZnMUE1M9QlBHdQaKjAOPvT8kQrn1BJ/iuyg==", "e08eb8f3-c07a-44b0-a40f-38f77aa9fd5e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bfc89bf9-8f34-4982-a684-d8c0b348d4f6", "Linda", "Michaels", "AQAAAAEAACcQAAAAEP3qwNOTeE5kS45WxQXBAe6ZNxHp+N8an5Q5rTAEs5k1Q1sybJ+GigTL5oErb9IgxA==", "0bc0d562-ee6d-460c-943b-82cdce348749" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b186cd92-5a87-4461-afdf-f4d74144c7fb", "AQAAAAEAACcQAAAAECwr1r8L3k4Hms8r0Bv3J30wkg9JR9relKoR65FQbvpF4SHFGcSPjV5ErEnmGwbtmg==", "abd54956-df73-48c2-9c62-2190b160a530" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4686843f-d525-42c6-bbc7-e8502a7b6018", "AQAAAAEAACcQAAAAEMAXCwSHYzo3Q9P/pCLro5BNd1mxSNAWeqHMpd14HKeYTMnucgjGEtQRajasCUe4eA==", "08d9794a-e20f-4930-85c3-28ad6a56ba80" });
        }
    }
}
