using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back___Api_integrada_MySql.Migrations
{
    /// <inheritdoc />
    public partial class adicionandoAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "Clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "Clientes");
        }
    }
}
