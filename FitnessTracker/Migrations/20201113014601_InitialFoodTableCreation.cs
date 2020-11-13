using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class InitialFoodTableCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFoods",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Calories = table.Column<int>(nullable: false),
                    Carbohydrates = table.Column<int>(nullable: false),
                    Protein = table.Column<int>(nullable: false),
                    Fat = table.Column<int>(nullable: false),
                    ServingSize = table.Column<int>(nullable: false),
                    ServingUnit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFoods", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserFoods_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FoodRecords",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodID = table.Column<long>(nullable: true),
                    ConsumptionDate = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodRecords", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FoodRecords_UserFoods_FoodID",
                        column: x => x.FoodID,
                        principalTable: "UserFoods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodRecords_FoodID",
                table: "FoodRecords",
                column: "FoodID");

            migrationBuilder.CreateIndex(
                name: "IX_UserFoods_CreatedById",
                table: "UserFoods",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodRecords");

            migrationBuilder.DropTable(
                name: "UserFoods");
        }
    }
}
