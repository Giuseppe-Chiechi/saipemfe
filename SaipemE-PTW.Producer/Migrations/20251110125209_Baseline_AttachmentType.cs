using Microsoft.EntityFrameworkCore.Migrations;
using System; // Added for DateTime

#nullable disable

namespace SaipemEPTW.Producer.Migrations
{
    /// <inheritdoc />
    public partial class Baseline_AttachmentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new audit columns to AttachmentTypes
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "AttachmentTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AttachmentTypes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "AttachmentTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AttachmentTypes");
        }
    }
}
