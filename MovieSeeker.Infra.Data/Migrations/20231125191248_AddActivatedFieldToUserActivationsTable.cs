using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieSeeker.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActivatedFieldToUserActivationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activated",
                table: "UserActivations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activated",
                table: "UserActivations");
        }
    }
}
