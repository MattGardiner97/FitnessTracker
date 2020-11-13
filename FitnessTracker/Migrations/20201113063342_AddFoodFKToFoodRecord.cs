using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class AddFoodFKToFoodRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRecords_UserFoods_FoodID",
                table: "FoodRecords");

            migrationBuilder.AlterColumn<long>(
                name: "FoodID",
                table: "FoodRecords",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRecords_UserFoods_FoodID",
                table: "FoodRecords",
                column: "FoodID",
                principalTable: "UserFoods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRecords_UserFoods_FoodID",
                table: "FoodRecords");

            migrationBuilder.AlterColumn<long>(
                name: "FoodID",
                table: "FoodRecords",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRecords_UserFoods_FoodID",
                table: "FoodRecords",
                column: "FoodID",
                principalTable: "UserFoods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
