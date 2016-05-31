using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicList1
{
    public class TasksContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Nivel> Niveis { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Tasks.db");
        }
    }

    public class Task
    {
        public int TaskId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string ImagemURL { get; set; }
        public string Categorias { get; set; }
        public DateTime Data { get; set; }
    }

    public class Nivel
    {
        public int NivelId { get; set; }
        public string Texto { get; set; }
        public string Funcionalidade { get; set; }
        public int NroNivel { get; set; }
        public int NroAtividades { get; set; }
    }

    public class Categoria
    {
        public int CategoriaID { get; set; }
        public string Descricao { get; set; }
        public bool IsRemovivel { get; set; }
    }
}
