using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jwt.DataAccessLayer.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PackageId",
                table: "Users",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PackageId",
                table: "Songs",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Packages_PackageId",
                table: "Songs",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Packages_PackageId",
                table: "Users",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Packages_PackageId",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Packages_PackageId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Users_PackageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Songs_PackageId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Songs");
        }
    }
}
