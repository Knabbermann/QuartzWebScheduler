using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuartzWebScheduler.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedRequestProperitiesToQuartzJobConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "QuartzJobConfigs",
                newName: "RequestUrl");

            migrationBuilder.AddColumn<string>(
                name: "RequestBody",
                table: "QuartzJobConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestType",
                table: "QuartzJobConfigs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestBody",
                table: "QuartzJobConfigs");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "QuartzJobConfigs");

            migrationBuilder.RenameColumn(
                name: "RequestUrl",
                table: "QuartzJobConfigs",
                newName: "Url");
        }
    }
}
