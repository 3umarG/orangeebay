using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddTicketTypeIdtoTicketDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketTypeId",
                table: "TicketsDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TicketsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e4712b59-2cc7-4521-a65d-e92ea51b8774");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "359d356e-2e56-4dc7-a69f-e2806dc71d32");

            migrationBuilder.InsertData(
                table: "TicketsTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Single" },
                    { 2, "Double" },
                    { 3, "Family" },
                    { 4, "Group" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketsDetails_TicketTypeId",
                table: "TicketsDetails",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsDetails_TicketsTypes_TicketTypeId",
                table: "TicketsDetails",
                column: "TicketTypeId",
                principalTable: "TicketsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsDetails_TicketsTypes_TicketTypeId",
                table: "TicketsDetails");

            migrationBuilder.DropTable(
                name: "TicketsTypes");

            migrationBuilder.DropIndex(
                name: "IX_TicketsDetails_TicketTypeId",
                table: "TicketsDetails");

            migrationBuilder.DropColumn(
                name: "TicketTypeId",
                table: "TicketsDetails");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0e354866-1cc2-4bae-b395-ab11519521e2");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ab0e29b9-d581-4084-a5bd-e3b04c6468d1");
        }
    }
}
