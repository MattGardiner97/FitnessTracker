using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class FoodAndRecordFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFoods_AspNetUsers_CreatedById",
                table: "UserFoods");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "UserFoods",
                newName: "CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_UserFoods_CreatedById",
                table: "UserFoods",
                newName: "IX_UserFoods_CreatedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoods_AspNetUsers_CreatedByID",
                table: "UserFoods",
                column: "CreatedByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFoods_AspNetUsers_CreatedByID",
                table: "UserFoods");

            migrationBuilder.RenameColumn(
                name: "CreatedByID",
                table: "UserFoods",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_UserFoods_CreatedByID",
                table: "UserFoods",
                newName: "IX_UserFoods_CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoods_AspNetUsers_CreatedById",
                table: "UserFoods",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
