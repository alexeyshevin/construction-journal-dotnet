using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConstructionJournal.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "work_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "work_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    work_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    work_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    unit = table.Column<string>(type: "text", nullable: false),
                    performer_name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_logs_work_types_work_type_id",
                        column: x => x.work_type_id,
                        principalTable: "work_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "work_types",
                columns: new[] { "Id", "name", "unit" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Бетонные работы", "м³" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Кладочные работы", "м²" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Земляные работы", "м³" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_work_logs_work_type_id",
                table: "work_logs",
                column: "work_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "work_logs");

            migrationBuilder.DropTable(
                name: "work_types");
        }
    }
}
