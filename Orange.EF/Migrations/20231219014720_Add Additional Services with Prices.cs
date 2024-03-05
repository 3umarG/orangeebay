using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddAdditionalServiceswithPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricePerChild = table.Column<double>(type: "float", nullable: false),
                    PricePerAdult = table.Column<double>(type: "float", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalServicePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalServicePrices_AdditionalServices_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "AdditionalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdditionalServicePrices_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdditionalServices",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Extra Boat From Beach", "Boat" },
                    { 2, "For Weddings", "Photo session" }
                });

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

            migrationBuilder.InsertData(
                table: "AdditionalServicePrices",
                columns: new[] { "Id", "PricePerAdult", "PricePerChild", "ServiceId", "UserTypeId" },
                values: new object[,]
                {
                    { 1, 250.0, 150.0, 1, 1 },
                    { 2, 350.0, 200.0, 1, 2 },
                    { 3, 800.0, 50.0, 2, 1 },
                    { 4, 400.0, 130.0, 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalServicePrices_ServiceId",
                table: "AdditionalServicePrices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalServicePrices_UserTypeId",
                table: "AdditionalServicePrices",
                column: "UserTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalServicePrices");

            migrationBuilder.DropTable(
                name: "AdditionalServices");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ad12e78e-0af4-4286-a039-b910e0b69a09");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "aa9e8d25-3aeb-442b-b03d-6826b56e5ad3");
        }
    }
}
