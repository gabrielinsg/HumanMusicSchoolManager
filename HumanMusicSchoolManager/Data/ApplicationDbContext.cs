using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HumanMusicSchoolManager.Models;
using HumanMusicSchoolManager.Models.Models;

namespace HumanMusicSchoolManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<DiarioClasse> DiariosClasse { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<RespFinanceiro> RespsFinanceiro { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<CursoProfessor>()
                .HasKey(cp => new { cp.ProfessorId, cp.CursoId });

            builder.Entity<CursoSala>()
                .HasKey(cs => new { cs.SalaId, cs.CursoId });

            builder.Entity<Matricula>()
                .HasOne(m => m.Professor)
                .WithMany()
                .HasForeignKey("ProfessorId")
                .OnDelete(DeleteBehavior.Restrict);
               

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
