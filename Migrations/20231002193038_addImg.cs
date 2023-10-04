using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOSPets.Migrations
{
    /// <inheritdoc />
    public partial class addImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Encontrado",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Desaparecido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Encontrado");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Desaparecido");
        }
    }
}
