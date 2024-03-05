using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddindextoComapyImagestitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CompaniesImages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompaniesImages_Title",
                table: "CompaniesImages");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CompaniesImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "914b9743-9c7d-42c3-9a34-5ea7366186aa");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0f47e328-2e31-4205-ba31-83d3e4e78ba2");
        }
    }
}
