using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class UpdateFoodRecordAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRecords_AspNetUsers_UserId",
                table: "FoodRecords");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FoodRecords",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRecords_AspNetUsers_UserId",
                table: "FoodRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRecords_AspNetUsers_UserId",
                table: "FoodRecords");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FoodRecords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRecords_AspNetUsers_UserId",
                table: "FoodRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
