﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOSPets.Migrations
{
    /// <inheritdoc />
    public partial class adminuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_Admin",
                table: "Usuario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Admin",
                table: "Usuario");
        }
    }
}
