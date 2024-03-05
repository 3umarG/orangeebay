using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NumberOfAdults = table.Column<int>(type: "int", nullable: false),
                    PricePerAdult = table.Column<double>(type: "float", nullable: false),
                    NumberOfChild = table.Column<int>(type: "int", nullable: false),
                    PricePerChild = table.Column<double>(type: "float", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationAdditionalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfAdults = table.Column<int>(type: "int", nullable: false),
                    PricePerAdult = table.Column<double>(type: "float", nullable: false),
                    NumberOfChild = table.Column<int>(type: "int", nullable: false),
                    PricePerChild = table.Column<double>(type: "float", nullable: false),
                    AdditionalServiceId = table.Column<int>(type: "int", nullable: false),
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationAdditionalServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationAdditionalServices_AdditionalServices_AdditionalServiceId",
                        column: x => x.AdditionalServiceId,
                        principalTable: "AdditionalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationAdditionalServices_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "58419890-466c-42b0-8590-e2de4f960bb7");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "caf9b533-46f3-4f44-ba00-92a658ee12cd");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationAdditionalServices_AdditionalServiceId",
                table: "ReservationAdditionalServices",
                column: "AdditionalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationAdditionalServices_ReservationId",
                table: "ReservationAdditionalServices",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ProgramId",
                table: "Reservations",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationAdditionalServices");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c0ebf318-9e79-40f5-8357-660807fd165f");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "33aef6ce-0b07-44c5-9bf2-fa51a87af64b");
        }
    }
}
