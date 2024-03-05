using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddDining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiningCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiningCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiningImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiningImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiningItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiningCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiningItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiningItems_DiningCategories_DiningCategoryId",
                        column: x => x.DiningCategoryId,
                        principalTable: "DiningCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DiningCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Restaurant" },
                    { 2, "Bar" },
                    { 3, "Lounges" }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "63c7b129-126a-4d1c-ab85-9f0827641f31");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "138571fc-b002-4fe1-ac39-b3673d88983f");

            migrationBuilder.CreateIndex(
                name: "IX_DiningItems_DiningCategoryId",
                table: "DiningItems",
                column: "DiningCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiningImages");

            migrationBuilder.DropTable(
                name: "DiningItems");

            migrationBuilder.DropTable(
                name: "DiningCategories");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9ec560e0-4b14-446c-9135-e81053a936e0");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1109bba8-7f65-4f58-a39e-e217edf63e40");
        }
    }
}
