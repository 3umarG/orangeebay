using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.EF.Migrations
{
    public partial class AddGalleryImageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "GalleryImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "GalleryImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GalleryImageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryImageTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GalleryImageTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Panoramic View" },
                    { 2, "Relax" },
                    { 3, "Joy and Fun" },
                    { 4, "Dining" }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f44daff6-5f09-4da5-9eef-e7f70adbca65");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a8fd3948-5a69-4ca1-ac2a-500bfe0f2eb5");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImages_TypeId",
                table: "GalleryImages",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryImages_GalleryImageTypes_TypeId",
                table: "GalleryImages",
                column: "TypeId",
                principalTable: "GalleryImageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GalleryImages_GalleryImageTypes_TypeId",
                table: "GalleryImages");

            migrationBuilder.DropTable(
                name: "GalleryImageTypes");

            migrationBuilder.DropIndex(
                name: "IX_GalleryImages_TypeId",
                table: "GalleryImages");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "GalleryImages");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "GalleryImages");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9da32c61-7d7a-4a7c-a1a5-a62cf0c38650");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3fa1ad4d-e6b5-445a-bcf9-2ab073603a83");
        }
    }
}
