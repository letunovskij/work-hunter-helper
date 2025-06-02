using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WorkHunter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    ResponsibleId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Completed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsCompletedByHand = table.Column<bool>(type: "boolean", nullable: false),
                    CompletionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Text = table.Column<string>(type: "text", nullable: false),
                    LastNotificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WResponseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTasks_AspNetUsers_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTasks_UserTaskTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "UserTaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTasks_WResponses_WResponseId",
                        column: x => x.WResponseId,
                        principalTable: "WResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_ResponsibleId",
                table: "UserTasks",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_TypeId",
                table: "UserTasks",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_WResponseId",
                table: "UserTasks",
                column: "WResponseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTasks");
        }
    }
}
