using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddCreateOntoTicketDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "TicketsDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TicketImages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d923a7a4-ff5a-400f-8c3a-22e9bfd98a5e");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "665b08de-c3c4-4201-b956-c403f637cb1f");

            migrationBuilder.CreateIndex(
                name: "IX_TicketsDetails_CreatedOn",
                table: "TicketsDetails",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_TicketImages_Title",
                table: "TicketImages",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketsDetails_CreatedOn",
                table: "TicketsDetails");

            migrationBuilder.DropIndex(
                name: "IX_TicketImages_Title",
                table: "TicketImages");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "TicketsDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TicketImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "874d4b3b-4d74-4b49-afbe-06d2c5c18358");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c7740a32-e297-405b-9e72-ad5589f9db08");
        }
    }
}
