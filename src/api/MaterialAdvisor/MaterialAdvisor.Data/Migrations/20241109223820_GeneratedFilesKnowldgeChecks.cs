using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialAdvisor.Data.Migrations
{
    /// <inheritdoc />
    public partial class GeneratedFilesKnowldgeChecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneratedFilesKnowldgeChecks",
                columns: table => new
                {
                    GeneratedFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KnowledgeCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedFilesKnowldgeChecks", x => new { x.GeneratedFileId, x.KnowledgeCheckId });
                    table.ForeignKey(
                        name: "FK_GeneratedFilesKnowldgeChecks_GeneratedFiles_GeneratedFileId",
                        column: x => x.GeneratedFileId,
                        principalTable: "GeneratedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneratedFilesKnowldgeChecks_KnowledgeChecks_KnowledgeCheckId",
                        column: x => x.KnowledgeCheckId,
                        principalTable: "KnowledgeChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedFilesKnowldgeChecks_GeneratedFileId",
                table: "GeneratedFilesKnowldgeChecks",
                column: "GeneratedFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedFilesKnowldgeChecks_KnowledgeCheckId",
                table: "GeneratedFilesKnowldgeChecks",
                column: "KnowledgeCheckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneratedFilesKnowldgeChecks");
        }
    }
}
