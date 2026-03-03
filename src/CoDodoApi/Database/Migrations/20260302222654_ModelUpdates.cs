using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoDodoApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_processes_opportunities_opportunity_uri",
                table: "processes");

            migrationBuilder.DropIndex(
                name: "ix_processes_opportunity_uri",
                table: "processes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_opportunities",
                table: "opportunities");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                table: "processes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "processes",
                newName: "created_at");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "processes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "opportunity_id",
                table: "processes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<decimal>(
                name: "hourly_rate_in_sek",
                table: "opportunities",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "opportunities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "opportunities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "opportunities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "pk_opportunities",
                table: "opportunities",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_processes_opportunity_id",
                table: "processes",
                column: "opportunity_id");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_id",
                table: "opportunities",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_processes_opportunities_opportunity_id",
                table: "processes",
                column: "opportunity_id",
                principalTable: "opportunities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_processes_opportunities_opportunity_id",
                table: "processes");

            migrationBuilder.DropIndex(
                name: "ix_processes_opportunity_id",
                table: "processes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_opportunities",
                table: "opportunities");

            migrationBuilder.DropIndex(
                name: "ix_opportunities_id",
                table: "opportunities");

            migrationBuilder.DropColumn(
                name: "id",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "opportunity_id",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "id",
                table: "opportunities");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "opportunities");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "opportunities");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "processes",
                newName: "updated_date");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "processes",
                newName: "created_date");

            migrationBuilder.AlterColumn<int>(
                name: "hourly_rate_in_sek",
                table: "opportunities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "pk_opportunities",
                table: "opportunities",
                column: "uri_for_assignment");

            migrationBuilder.CreateIndex(
                name: "ix_processes_opportunity_uri",
                table: "processes",
                column: "opportunity_uri");

            migrationBuilder.AddForeignKey(
                name: "fk_processes_opportunities_opportunity_uri",
                table: "processes",
                column: "opportunity_uri",
                principalTable: "opportunities",
                principalColumn: "uri_for_assignment",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
