using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orange.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramsWithoutPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialRequirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationInHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramImages_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramPlans_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateFromFive = table.Column<double>(type: "float", nullable: false),
                    ReviewDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramReviews_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "Id", "Description", "DurationInHours", "InternalNotes", "Location", "MaxCapacity", "Name", "SpecialRequirements" },
                values: new object[,]
                {
                    { 1, "Description for Program 1", 12, "Notes .........", "Orange Island", 25, "Program 1", "Requirements .........." },
                    { 2, "Description for Program 2", 32, "Notes .........", "Orange Island", 12, "Program 2", "Requirements .........." }
                });

            migrationBuilder.InsertData(
                table: "ProgramPlans",
                columns: new[] { "Id", "Description", "ProgramId", "Time" },
                values: new object[,]
                {
                    { 1, "Start the Trip", 1, "09:00 AM" },
                    { 2, "Sea", 1, "03:00 AM" },
                    { 3, "Evening", 1, "10:00 PM" },
                    { 4, "Ending", 1, "12:00 PM" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImages_ProgramId",
                table: "ProgramImages",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramPlans_ProgramId",
                table: "ProgramPlans",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramReviews_ProgramId",
                table: "ProgramReviews",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramReviews_UserId",
                table: "ProgramReviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramImages");

            migrationBuilder.DropTable(
                name: "ProgramPlans");

            migrationBuilder.DropTable(
                name: "ProgramReviews");

            migrationBuilder.DropTable(
                name: "Programs");
        }
    }
}
