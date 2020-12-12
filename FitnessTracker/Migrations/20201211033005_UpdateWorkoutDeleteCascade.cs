using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessTracker.Migrations
{
    public partial class UpdateWorkoutDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutActivities_WorkoutSessions_WorkoutSessionID",
                table: "WorkoutActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSessions_WorkoutPlans_WorkoutPlanID",
                table: "WorkoutSessions");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutSessions_WorkoutPlanID",
                table: "WorkoutSessions");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutActivities_WorkoutSessionID",
                table: "WorkoutActivities");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanID",
                table: "WorkoutSessions");

            migrationBuilder.DropColumn(
                name: "WorkoutSessionID",
                table: "WorkoutActivities");

            migrationBuilder.AddColumn<long>(
                name: "PlanID",
                table: "WorkoutSessions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SessionID",
                table: "WorkoutActivities",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_PlanID",
                table: "WorkoutSessions",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutActivities_SessionID",
                table: "WorkoutActivities",
                column: "SessionID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutActivities_WorkoutSessions_SessionID",
                table: "WorkoutActivities",
                column: "SessionID",
                principalTable: "WorkoutSessions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSessions_WorkoutPlans_PlanID",
                table: "WorkoutSessions",
                column: "PlanID",
                principalTable: "WorkoutPlans",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutActivities_WorkoutSessions_SessionID",
                table: "WorkoutActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSessions_WorkoutPlans_PlanID",
                table: "WorkoutSessions");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutSessions_PlanID",
                table: "WorkoutSessions");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutActivities_SessionID",
                table: "WorkoutActivities");

            migrationBuilder.DropColumn(
                name: "PlanID",
                table: "WorkoutSessions");

            migrationBuilder.DropColumn(
                name: "SessionID",
                table: "WorkoutActivities");

            migrationBuilder.AddColumn<long>(
                name: "WorkoutPlanID",
                table: "WorkoutSessions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkoutSessionID",
                table: "WorkoutActivities",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_WorkoutPlanID",
                table: "WorkoutSessions",
                column: "WorkoutPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutActivities_WorkoutSessionID",
                table: "WorkoutActivities",
                column: "WorkoutSessionID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutActivities_WorkoutSessions_WorkoutSessionID",
                table: "WorkoutActivities",
                column: "WorkoutSessionID",
                principalTable: "WorkoutSessions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSessions_WorkoutPlans_WorkoutPlanID",
                table: "WorkoutSessions",
                column: "WorkoutPlanID",
                principalTable: "WorkoutPlans",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
