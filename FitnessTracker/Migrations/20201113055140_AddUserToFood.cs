using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class AddUserToFood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FoodRecords",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodRecords_UserId",
                table: "FoodRecords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRecords_AspNetUsers_UserId",
                table: "FoodRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRecords_AspNetUsers_UserId",
                table: "FoodRecords");

            migrationBuilder.DropIndex(
                name: "IX_FoodRecords_UserId",
                table: "FoodRecords");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FoodRecords");
        }
    }
}
