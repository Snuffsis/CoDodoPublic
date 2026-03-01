using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoDodoApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "opportunities",
                columns: table => new
                {
                    uri_for_assignment = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: false),
                    capability = table.Column<string>(type: "text", nullable: false),
                    name_of_sales_lead = table.Column<string>(type: "text", nullable: false),
                    hourly_rate_in_sek = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_opportunities", x => x.uri_for_assignment);
                });

            migrationBuilder.CreateTable(
                name: "processes",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    opportunity_uri = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_processes", x => new { x.name, x.opportunity_uri });
                    table.ForeignKey(
                        name: "fk_processes_opportunities_opportunity_uri",
                        column: x => x.opportunity_uri,
                        principalTable: "opportunities",
                        principalColumn: "uri_for_assignment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_uri_for_assignment",
                table: "opportunities",
                column: "uri_for_assignment",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_processes_opportunity_uri",
                table: "processes",
                column: "opportunity_uri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "processes");

            migrationBuilder.DropTable(
                name: "opportunities");
        }
    }
}
