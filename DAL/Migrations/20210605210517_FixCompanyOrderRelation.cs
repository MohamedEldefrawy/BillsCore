using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixCompanyOrderRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Companies_CompaniesID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CompaniesID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CompaniesID",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompaniesID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CompaniesID",
                table: "Orders",
                column: "CompaniesID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Companies_CompaniesID",
                table: "Orders",
                column: "CompaniesID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
