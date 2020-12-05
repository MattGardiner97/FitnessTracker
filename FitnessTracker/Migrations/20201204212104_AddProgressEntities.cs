using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class AddProgressEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoalProgressRecords",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    GoalID = table.Column<long>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Quantity = table.Column<float>(nullable: true),
                    Time = table.Column<TimeSpan>(nullable: true),
                    Weight = table.Column<float>(nullable: true),
                    Reps = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalProgressRecords", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GoalProgressRecords_Goals_GoalID",
                        column: x => x.GoalID,
                        principalTable: "Goals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoalProgressRecords_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoalProgressRecords_GoalID",
                table: "GoalProgressRecords",
                column: "GoalID");

            migrationBuilder.CreateIndex(
                name: "IX_GoalProgressRecords_UserId",
                table: "GoalProgressRecords",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoalProgressRecords");
        }
    }
}
