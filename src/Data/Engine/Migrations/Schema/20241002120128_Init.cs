using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Data.Engine.Migrations.Schema
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tbot");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "tbot",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    normalized_name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "tbot",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    telegram_user_id = table.Column<string>(type: "text", nullable: false),
                    language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    auto_detect_language = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                schema: "tbot",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    chat_id = table.Column<string>(type: "text", nullable: false),
                    message_id = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    start_parameter = table.Column<string>(type: "text", nullable: false),
                    telegram_payment_charge_id = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "tbot",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_commands",
                schema: "tbot",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    command_name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_commands", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_commands_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "tbot",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "tbot",
                columns: table => new
                {
                    roles_id = table.Column<string>(type: "character varying(255)", nullable: false),
                    users_id = table.Column<string>(type: "character varying(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_user_role_roles_roles_id",
                        column: x => x.roles_id,
                        principalSchema: "tbot",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_role_users_users_id",
                        column: x => x.users_id,
                        principalSchema: "tbot",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_invoices_chat_id",
                schema: "tbot",
                table: "invoices",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_type",
                schema: "tbot",
                table: "invoices",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_user_id",
                schema: "tbot",
                table: "invoices",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_commands_created_at",
                schema: "tbot",
                table: "user_commands",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_user_commands_user_id",
                schema: "tbot",
                table: "user_commands",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_users_id",
                schema: "tbot",
                table: "user_role",
                column: "users_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_telegram_user_id",
                schema: "tbot",
                table: "users",
                column: "telegram_user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoices",
                schema: "tbot");

            migrationBuilder.DropTable(
                name: "user_commands",
                schema: "tbot");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "tbot");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "tbot");

            migrationBuilder.DropTable(
                name: "users",
                schema: "tbot");
        }
    }
}
