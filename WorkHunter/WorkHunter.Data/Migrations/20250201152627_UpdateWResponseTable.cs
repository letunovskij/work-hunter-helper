using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWResponseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.CreateTable(
                name: "WResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    HhId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAnswered = table.Column<bool>(type: "boolean", nullable: false),
                    ViewedByMe = table.Column<bool>(type: "boolean", nullable: false),
                    Contact = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    AnswerText = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VacancyUrl = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WResponses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WResponses_UserId",
                table: "WResponses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WResponses");

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AnswerText = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Contact = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HhId = table.Column<string>(type: "text", nullable: true),
                    IsAnswered = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VacancyUrl = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    ViewedByMe = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responses_UserId",
                table: "Responses",
                column: "UserId");
        }
    }
}
