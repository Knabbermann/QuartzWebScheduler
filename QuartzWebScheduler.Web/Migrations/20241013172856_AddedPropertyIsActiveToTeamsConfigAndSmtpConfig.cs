using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuartzWebScheduler.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyIsActiveToTeamsConfigAndSmtpConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TeamsConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SmtpConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TeamsConfigs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SmtpConfigs");
        }
    }
}
