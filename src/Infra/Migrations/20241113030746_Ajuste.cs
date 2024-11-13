using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Ajuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                schema: "dbo",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "Pedidos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClienteId",
                schema: "dbo",
                table: "Pedidos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Pedidos",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");
        }
    }
}
