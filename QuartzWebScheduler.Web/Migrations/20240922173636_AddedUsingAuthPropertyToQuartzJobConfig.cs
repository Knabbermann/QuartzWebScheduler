using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuartzWebScheduler.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsingAuthPropertyToQuartzJobConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsingAuth",
                table: "QuartzJobConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsingAuth",
                table: "QuartzJobConfigs");
        }
    }
}
