using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "disciplines",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disciplines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.CheckConstraint("users_role_check", "\"role\" IN ('Student', 'Teacher', 'HeadOfDepartment')");
                });

            migrationBuilder.CreateTable(
                name: "retake_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    discipline_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_user_id = table.Column<int>(type: "integer", nullable: false),
                    assignee_user_id = table.Column<int>(type: "integer", nullable: true),
                    retake_date = table.Column<DateOnly>(type: "date", nullable: true),
                    retake_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    audience = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retake_requests", x => x.id);
                    table.CheckConstraint("retake_requests_description_check", "char_length(description) BETWEEN 10 AND 500");
                    table.CheckConstraint("retake_requests_number_check", "\"number\" > 0");
                    table.CheckConstraint("retake_requests_status_check", "\"status\" IN ('New', 'InProgress', 'Closed', 'Cancelled')");
                    table.CheckConstraint("retake_requests_title_check", "char_length(title) BETWEEN 5 AND 80");
                    table.ForeignKey(
                        name: "FK_retake_requests_disciplines_discipline_id",
                        column: x => x.discipline_id,
                        principalTable: "disciplines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_retake_requests_users_assignee_user_id",
                        column: x => x.assignee_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_retake_requests_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_disciplines_name",
                table: "disciplines",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_retake_requests_assignee_user_id",
                table: "retake_requests",
                column: "assignee_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_retake_requests_created_by_user_id",
                table: "retake_requests",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_retake_requests_number",
                table: "retake_requests",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "retake_requests_discipline_id_idx",
                table: "retake_requests",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "retake_requests_status_idx",
                table: "retake_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "retake_requests");

            migrationBuilder.DropTable(
                name: "disciplines");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
