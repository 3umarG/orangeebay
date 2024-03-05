using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class Deleteallimagesfromdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketImages_Title",
                table: "TicketImages");

            migrationBuilder.DropIndex(
                name: "IX_SliderImages_Title",
                table: "SliderImages");

            migrationBuilder.DropIndex(
                name: "IX_ProgramImages_Title",
                table: "ProgramImages");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "TicketImages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TicketImages");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "SliderImages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SliderImages");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "ProgramImages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProgramImages");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "DiningImages");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c62c66c5-0728-447b-b9ca-f458b5a8ddff");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f5af373b-ade0-4ce1-b556-2a498c80ec42");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "TicketImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TicketImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "SliderImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SliderImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "ProgramImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProgramImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "DiningImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "18bbe40c-d73f-44c2-9376-5239fbb9bf54");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d359ce5d-ef2b-45dd-8a80-561778197a9a");

            migrationBuilder.CreateIndex(
                name: "IX_TicketImages_Title",
                table: "TicketImages",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_SliderImages_Title",
                table: "SliderImages",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImages_Title",
                table: "ProgramImages",
                column: "Title");
        }
    }
}
