using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddIndexonBookingDateinReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "15f82766-4e3f-40d2-9f46-8a87cabba6d1");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b5f0173c-1fa2-4497-9d21-d8f8e6cb9c3d");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookingDate",
                table: "Reservations",
                column: "BookingDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_BookingDate",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7b70961b-d517-45ba-89b2-b6b52169d2c2");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "bfa540f9-e2b6-4ffc-a53e-bfb781ce01f2");
        }
    }
}
