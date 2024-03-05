using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class Deleteusersimagestables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CultureTypes_CultureTypeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CultureTypes",
                table: "CultureTypes");

            migrationBuilder.RenameTable(
                name: "CultureTypes",
                newName: "CultureType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CultureType",
                table: "CultureType",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7dc5ce81-7af1-4d20-b6f1-eb0cebd600e7");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9cdeb580-54fd-4604-ac1e-e450692d0ac0");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CultureType_CultureTypeId",
                table: "Users",
                column: "CultureTypeId",
                principalTable: "CultureType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CultureType_CultureTypeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CultureType",
                table: "CultureType");

            migrationBuilder.RenameTable(
                name: "CultureType",
                newName: "CultureTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CultureTypes",
                table: "CultureTypes",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CultureTypes_CultureTypeId",
                table: "Users",
                column: "CultureTypeId",
                principalTable: "CultureTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
