using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaipemEPTW.Producer.Migrations
{
    /// <inheritdoc />
    public partial class Add_LogMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogMessages",
                columns: table => new
                {
                    CorrelationId = table.Column<string>(type: "nvarchar(100)", maxLength:100, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength:20, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength:2000, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(200)", maxLength:200, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(2000)", maxLength:2000, nullable: true),
                    ExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientInfo = table.Column<string>(type: "nvarchar(500)", maxLength:500, nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogMessages", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogMessages");
        }
    }
}
