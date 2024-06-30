using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialAdvisor.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageTexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LanguageId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageTexts_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageTextEntityTopicEntity",
                columns: table => new
                {
                    TextsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTextEntityTopicEntity", x => new { x.TextsId, x.TopicsId });
                    table.ForeignKey(
                        name: "FK_LanguageTextEntityTopicEntity_LanguageTexts_TextsId",
                        column: x => x.TextsId,
                        principalTable: "LanguageTexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageTextEntityTopicEntity_Topics_TopicsId",
                        column: x => x.TopicsId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<byte>(type: "tinyint", nullable: false),
                    Version = table.Column<byte>(type: "tinyint", nullable: false),
                    Points = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerGroups_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageTextEntityQuestionEntity",
                columns: table => new
                {
                    QuestionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTextEntityQuestionEntity", x => new { x.QuestionsId, x.TextsId });
                    table.ForeignKey(
                        name: "FK_LanguageTextEntityQuestionEntity_LanguageTexts_TextsId",
                        column: x => x.TextsId,
                        principalTable: "LanguageTexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageTextEntityQuestionEntity_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerGroupEntityLanguageTextEntity",
                columns: table => new
                {
                    AnswerGroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerGroupEntityLanguageTextEntity", x => new { x.AnswerGroupsId, x.TextsId });
                    table.ForeignKey(
                        name: "FK_AnswerGroupEntityLanguageTextEntity_AnswerGroups_AnswerGroupsId",
                        column: x => x.AnswerGroupsId,
                        principalTable: "AnswerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerGroupEntityLanguageTextEntity_LanguageTexts_TextsId",
                        column: x => x.TextsId,
                        principalTable: "LanguageTexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<byte>(type: "tinyint", nullable: false),
                    Points = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_AnswerGroups_AnswerGroupId",
                        column: x => x.AnswerGroupId,
                        principalTable: "AnswerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerEntityLanguageTextEntity",
                columns: table => new
                {
                    AnswersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerEntityLanguageTextEntity", x => new { x.AnswersId, x.TextsId });
                    table.ForeignKey(
                        name: "FK_AnswerEntityLanguageTextEntity_Answers_AnswersId",
                        column: x => x.AnswersId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerEntityLanguageTextEntity_LanguageTexts_TextsId",
                        column: x => x.TextsId,
                        principalTable: "LanguageTexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerEntityLanguageTextEntity_TextsId",
                table: "AnswerEntityLanguageTextEntity",
                column: "TextsId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerGroupEntityLanguageTextEntity_TextsId",
                table: "AnswerGroupEntityLanguageTextEntity",
                column: "TextsId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerGroups_QuestionId",
                table: "AnswerGroups",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_AnswerGroupId",
                table: "Answers",
                column: "AnswerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTextEntityQuestionEntity_TextsId",
                table: "LanguageTextEntityQuestionEntity",
                column: "TextsId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTextEntityTopicEntity_TopicsId",
                table: "LanguageTextEntityTopicEntity",
                column: "TopicsId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTexts_LanguageId",
                table: "LanguageTexts",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicId",
                table: "Questions",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_UserId",
                table: "Topics",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerEntityLanguageTextEntity");

            migrationBuilder.DropTable(
                name: "AnswerGroupEntityLanguageTextEntity");

            migrationBuilder.DropTable(
                name: "LanguageTextEntityQuestionEntity");

            migrationBuilder.DropTable(
                name: "LanguageTextEntityTopicEntity");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "LanguageTexts");

            migrationBuilder.DropTable(
                name: "AnswerGroups");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
