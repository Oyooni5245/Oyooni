using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Oyooni.Server.Migrations
{
    public partial class AddAvailableTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6171a3b4-dcc0-4638-bcbe-9c3aff974442", "f4a4812b-4257-4a98-ae0b-09e45d61cf1b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6171a3b4-dcc0-4638-bcbe-9c3aff974442");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f4a4812b-4257-4a98-ae0b-09e45d61cf1b");

            migrationBuilder.CreateTable(
                name: "AvailableTimes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayOfWeekId = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailableTimes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5b509ba-ba60-4d99-95a7-77518c39a41c", "ef9ed498-aa6c-45b6-94ec-16d595b14a61", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dc7bb088-24cd-4cba-afb3-c6745990964a", 0, "f6e3ae1b-306c-4272-be59-0f050679cff8", "admin@oyooni.com", true, "Admin", null, false, null, "ADMIN@OYOONI.COM", "ADMIN@OYOONI.COM", "AQAAAAEAACcQAAAAEFZwR1tKI9ScuXAnOlOYoxNpRuFv2i7tuKzUVt8Kb0IKjTx6JBht4IBrbvqvpFqneg==", null, false, "8a79aa59-926d-44e2-8c0a-33b1d7839469", false, "admin@oyooni.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e5b509ba-ba60-4d99-95a7-77518c39a41c", "dc7bb088-24cd-4cba-afb3-c6745990964a" });

            migrationBuilder.CreateIndex(
                name: "IX_AvailableTimes_UserId",
                table: "AvailableTimes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableTimes");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e5b509ba-ba60-4d99-95a7-77518c39a41c", "dc7bb088-24cd-4cba-afb3-c6745990964a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5b509ba-ba60-4d99-95a7-77518c39a41c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dc7bb088-24cd-4cba-afb3-c6745990964a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6171a3b4-dcc0-4638-bcbe-9c3aff974442", "56376f84-aea0-494b-9478-c5a148d50ad7", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f4a4812b-4257-4a98-ae0b-09e45d61cf1b", 0, "a4033afa-a175-442d-9a9b-f0e7bee06fd8", "admin@oyooni.com", true, "Admin", null, false, null, "ADMIN@OYOONI.COM", "ADMIN@OYOONI.COM", "AQAAAAEAACcQAAAAEDNE0K79I2HXwxSX1pDavLDidTsIqR3MbB+gkhwDbPqDLtxUoeb7hUxgcLXuWATfNA==", null, false, "cf9e0397-835b-472e-971b-ac5a1d324b3d", false, "admin@oyooni.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "6171a3b4-dcc0-4638-bcbe-9c3aff974442", "f4a4812b-4257-4a98-ae0b-09e45d61cf1b" });
        }
    }
}
