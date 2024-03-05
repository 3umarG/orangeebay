using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class DeleteDatafromComapniesImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompaniesImages_Title",
                table: "CompaniesImages");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "CompaniesImages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CompaniesImages");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "CompaniesImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CompaniesImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "580ee8aa-920a-43a0-9986-ebc6c5d8637e");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a097c1a1-7f57-49e7-81ba-05e87c54f206");

            migrationBuilder.CreateIndex(
                name: "IX_CompaniesImages_Title",
                table: "CompaniesImages",
                column: "Title");
        }
    }
}
