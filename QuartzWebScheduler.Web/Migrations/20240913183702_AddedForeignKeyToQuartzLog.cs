using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuartzWebScheduler.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyToQuartzLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "QuartzLogs",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "QuartzJobConfigId",
                table: "QuartzLogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QuartzLogs_QuartzJobConfigId",
                table: "QuartzLogs",
                column: "QuartzJobConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuartzLogs_QuartzJobConfigs_QuartzJobConfigId",
                table: "QuartzLogs",
                column: "QuartzJobConfigId",
                principalTable: "QuartzJobConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuartzLogs_QuartzJobConfigs_QuartzJobConfigId",
                table: "QuartzLogs");

            migrationBuilder.DropIndex(
                name: "IX_QuartzLogs_QuartzJobConfigId",
                table: "QuartzLogs");

            migrationBuilder.DropColumn(
                name: "QuartzJobConfigId",
                table: "QuartzLogs");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "QuartzLogs",
                newName: "CreatedDate");
        }
    }
}
