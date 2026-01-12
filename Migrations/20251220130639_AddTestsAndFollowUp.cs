using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MHC_AFMS.Migrations
{
    /// <inheritdoc />
    public partial class AddTestsAndFollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpDate",
                table: "Appointments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedTests",
                table: "Appointments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowUpDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RecommendedTests",
                table: "Appointments");
        }
    }
}
