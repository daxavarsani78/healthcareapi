using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityCode", "SecurityStamp", "TwoFactorEnabled", "UPassword", "UpdatedOn", "UserName" },
                values: new object[] { "241de04e-275b-491b-958d-39a3a7e0faf9", 0, "1440ecd3-641f-4aa4-8580-ea07d52d62a0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "renishribadiya10@gmail.com", false, "Renis", true, "Ribadiya", false, null, null, null, null, "8128791896", false, null, "cd0b855e-e51b-4852-bc9d-758495b2400b", false, "Renish@321", null, "Renis Ribadiya" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "241de04e-275b-491b-958d-39a3a7e0faf9");
        }
    }
}
