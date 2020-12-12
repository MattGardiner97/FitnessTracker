using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class RemoveWorkoutSessionAndActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutActivities");

            migrationBuilder.DropTable(
                name: "WorkoutSessions");

            migrationBuilder.AddColumn<string>(
                name: "SessionsJSON",
                table: "WorkoutPlans",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionsJSON",
                table: "WorkoutPlans");

            migrationBuilder.CreateTable(
                name: "WorkoutSessions",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_WorkoutPlans_PlanID",
                        column: x => x.PlanID,
                        principalTable: "WorkoutPlans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutActivities",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestPeriodSeconds = table.Column<int>(type: "int", nullable: false),
                    SessionID = table.Column<long>(type: "bigint", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutActivities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkoutActivities_WorkoutSessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "WorkoutSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutActivities_SessionID",
                table: "WorkoutActivities",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_PlanID",
                table: "WorkoutSessions",
                column: "PlanID");
        }
    }
}
