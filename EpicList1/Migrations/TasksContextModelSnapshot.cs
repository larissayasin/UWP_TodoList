using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EpicList1;

namespace EpicList1.Migrations
{
    [DbContext(typeof(TasksContext))]
    partial class TasksContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20896");

            modelBuilder.Entity("EpicList1.Categoria", b =>
                {
                    b.Property<int>("CategoriaID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descricao");

                    b.Property<bool>("IsRemovivel");

                    b.HasKey("CategoriaID");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("EpicList1.Nivel", b =>
                {
                    b.Property<int>("NivelId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Funcionalidade");

                    b.Property<int>("NroAtividades");

                    b.Property<int>("NroNivel");

                    b.Property<string>("Texto");

                    b.HasKey("NivelId");

                    b.ToTable("Niveis");
                });

            modelBuilder.Entity("EpicList1.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Categorias");

                    b.Property<DateTime>("Data");

                    b.Property<string>("Descricao");

                    b.Property<string>("Imagem");

                    b.Property<string>("ImagemURL");

                    b.Property<string>("Titulo");

                    b.HasKey("TaskId");

                    b.ToTable("Tasks");
                });
        }
    }
}
