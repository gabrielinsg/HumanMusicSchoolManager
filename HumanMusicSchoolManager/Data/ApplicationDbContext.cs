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
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<RespFinanceiro> RespsFinanceiro { get; set; }
        public DbSet<DispSala> DispSalas { get; set; }
        public DbSet<PacoteAula> PacotesAula { get; set; }
        public DbSet<PacoteCompra> PacoteCompras { get; set; }
        public DbSet<TaxaMatricula> TaxaMatriculas { get; set; }
        public DbSet<Financeiro> Financeiros { get; set; }
        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Chamada> Chamadas { get; set; }
        public DbSet<Reposicao> Reposicoes { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Demostrativa> Demostrativas { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Trancamento> Trancamentos { get; set; }
        public DbSet<RelatorioMatricula> RelatorioMatriculas { get; set; }
        public DbSet<EmailConfig> EmailConfig { get; set; }
        public DbSet<AulaConfig> AulaConfig { get; set; }
        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<TransacaoCaixa> TransacoesCaixa { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Caixa>()
                .HasOne(c => c.FuncionarioAberto)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Caixa>()
                .HasOne(c => c.FuncionarioFechado)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TransacaoCaixa>()
                .HasOne(t => t.Funcionario)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CursoProfessor>()
                .HasKey(cp => new { cp.ProfessorId, cp.CursoId });

            builder.Entity<CursoSala>()
                .HasKey(cs => new { cs.SalaId, cs.CursoId });

            builder.Entity<Matricula>()
                .HasOne(m => m.RespFinanceiro)
                .WithMany()
                .HasForeignKey("RespFinanceiroId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Financeiro>()
                .HasOne(f => f.Pessoa)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sala>()
                .HasMany(s => s.DispSalas)
                .WithOne(ds => ds.Sala)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Matricula>()
                .HasMany(m => m.PacoteCompras)
                .WithOne(pc => pc.Matricula)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RespFinanceiro>()
                .HasMany(rf => rf.Matriculas)
                .WithOne(m => m.RespFinanceiro)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DispSala>()
                .HasMany(ds => ds.Matriculas)
                .WithOne(m => m.DispSala);

            builder.Entity<DispSala>()
                .HasMany(ds => ds.Reposicoes)
                .WithOne(r => r.DispSala)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DispSala>()
                .HasMany(ds => ds.Demostrativas)
                .WithOne(d => d.DispSala)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Curso>()
                .HasMany(c => c.Demostrativas)
                .WithOne(d => d.Curso)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reposicao>()
                .HasOne(r => r.Chamada)
                .WithOne(c => c.Reposicao);

            builder.Entity<PacoteAula>()
                .HasOne(p => p.Contrato)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Demostrativa>()
                .HasOne(d => d.Pessoa)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Demostrativa>()
                .HasOne(d => d.Professor)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RelatorioMatricula>()
                .HasOne(rm => rm.Pessoa)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);          

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
