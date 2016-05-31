using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EpicList1.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Descricao = table.Column<string>(nullable: true),
                    IsRemovivel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "Niveis",
                columns: table => new
                {
                    NivelId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Funcionalidade = table.Column<string>(nullable: true),
                    NroAtividades = table.Column<int>(nullable: false),
                    NroNivel = table.Column<int>(nullable: false),
                    Texto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Niveis", x => x.NivelId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Categorias = table.Column<string>(nullable: true),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    Imagem = table.Column<string>(nullable: true),
                    ImagemURL = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Niveis");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
