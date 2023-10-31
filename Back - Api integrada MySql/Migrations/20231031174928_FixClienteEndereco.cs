using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back___Api_integrada_MySql.Migrations
{
    /// <inheritdoc />
    public partial class FixClienteEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Endereco_EnderecoDeEntregaId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "EnderecoDeEntregaId",
                table: "Clientes",
                newName: "EnderecoId");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_EnderecoDeEntregaId",
                table: "Clientes",
                newName: "IX_Clientes_EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Endereco_EnderecoId",
                table: "Clientes",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Endereco_EnderecoId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "EnderecoId",
                table: "Clientes",
                newName: "EnderecoDeEntregaId");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_EnderecoId",
                table: "Clientes",
                newName: "IX_Clientes_EnderecoDeEntregaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Endereco_EnderecoDeEntregaId",
                table: "Clientes",
                column: "EnderecoDeEntregaId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
