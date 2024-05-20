using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boleto.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableBoleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Boletos",
                table: "Boletos");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Boletos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boletos",
                table: "Boletos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Boletos",
                table: "Boletos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Boletos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boletos",
                table: "Boletos",
                column: "BarCode");
        }
    }
}
