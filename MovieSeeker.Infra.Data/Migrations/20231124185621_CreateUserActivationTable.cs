using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieSeeker.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserActivationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserActivations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SendingMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActivationMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Expired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserActivations_UserId",
                table: "UserActivations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActivations");
        }
    }
}
