using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SistemaControleAlunos.Models
{
    // 2º step
    // cria a class ControleContext que herda de DbContext, usa Data.Entity, cria um construtor que herda da connection string
    // cria um metodo override para fechar o context
    // no nuget enable-migrations -ContextTypeName ControleContext -EnabledAutomaticMigrations -Force
    // no nuget
    public class ControleContext:DbContext
    {
        public ControleContext():base("DefaultConnection")
        {
            
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //32º step
        //cria o metodo que desabilita erro de cascata e erros de referncias circulares
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<SistemaControleAlunos.Models.Usuario> Usuarios { get; set; }

        public System.Data.Entity.DbSet<SistemaControleAlunos.Models.Grupos> Grupos { get; set; }

        public System.Data.Entity.DbSet<SistemaControleAlunos.Models.GruposDetalhes> GruposDetalheses { get; set; }

        public System.Data.Entity.DbSet<SistemaControleAlunos.Models.Notas> Notas { get; set; }
    }
}