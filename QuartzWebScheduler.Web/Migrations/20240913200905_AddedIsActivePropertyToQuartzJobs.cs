using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuartzWebScheduler.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActivePropertyToQuartzJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "QuartzJobConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "QuartzJobConfigs");
        }
    }
}
