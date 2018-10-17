﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HumanMusicSchoolManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181016202421_DispSala")]
    partial class DispSala
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HumanMusicSchoolManager.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<int>("PessoaId");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PessoaId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Curso", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Ativo");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<int>("QtdModulo");

                    b.HasKey("Id");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.CursoProfessor", b =>
                {
                    b.Property<int>("ProfessorId");

                    b.Property<int>("CursoId");

                    b.HasKey("ProfessorId", "CursoId");

                    b.HasIndex("CursoId");

                    b.ToTable("CursoProfessor");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.CursoSala", b =>
                {
                    b.Property<int>("SalaId");

                    b.Property<int>("CursoId");

                    b.HasKey("SalaId", "CursoId");

                    b.HasIndex("CursoId");

                    b.ToTable("CursoSala");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.DiarioClasse", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Data");

                    b.Property<string>("DescAtividades")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<int>("MatriculaId");

                    b.Property<bool>("Presenca");

                    b.HasKey("Id");

                    b.HasIndex("MatriculaId");

                    b.ToTable("DiariosClasse");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.DispSala", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Dia");

                    b.Property<int>("Hora");

                    b.Property<int?>("ProfessorId");

                    b.Property<int?>("SalaId");

                    b.HasKey("Id");

                    b.HasIndex("ProfessorId");

                    b.HasIndex("SalaId");

                    b.ToTable("DispSalas");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro")
                        .IsRequired();

                    b.Property<string>("CEP")
                        .IsRequired();

                    b.Property<string>("Cidade")
                        .IsRequired();

                    b.Property<string>("Complemento");

                    b.Property<string>("Logradouro")
                        .IsRequired();

                    b.Property<int>("Numero");

                    b.Property<int>("UF");

                    b.HasKey("Id");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Matricula", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlunoId");

                    b.Property<bool>("Ativo");

                    b.Property<int>("CursoId");

                    b.Property<DateTime>("DataMatricula");

                    b.Property<int>("Dia");

                    b.Property<int>("Hora");

                    b.Property<int>("ProfessorId");

                    b.Property<int?>("RespFinanceiroId");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId");

                    b.HasIndex("CursoId");

                    b.HasIndex("ProfessorId");

                    b.HasIndex("RespFinanceiroId");

                    b.ToTable("Matriculas");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Pessoa", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Ativo");

                    b.Property<string>("CPF")
                        .IsRequired();

                    b.Property<string>("Cel");

                    b.Property<DateTime>("DataNascimento");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email");

                    b.Property<int>("EnderecoId");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("RG")
                        .IsRequired();

                    b.Property<string>("Tel");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId");

                    b.ToTable("Pessoas");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Pessoa");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Sala", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Ativo");

                    b.Property<int>("Capacidade");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Salas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Aluno", b =>
                {
                    b.HasBaseType("HumanMusicSchoolManager.Models.Models.Pessoa");

                    b.Property<int>("RM");

                    b.Property<int?>("RespFinanceiroId");

                    b.HasIndex("RespFinanceiroId");

                    b.ToTable("Aluno");

                    b.HasDiscriminator().HasValue("Aluno");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Funcionario", b =>
                {
                    b.HasBaseType("HumanMusicSchoolManager.Models.Models.Pessoa");


                    b.ToTable("Funcionario");

                    b.HasDiscriminator().HasValue("Funcionario");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Professor", b =>
                {
                    b.HasBaseType("HumanMusicSchoolManager.Models.Models.Pessoa");

                    b.Property<decimal>("Salario");

                    b.ToTable("Professor");

                    b.HasDiscriminator().HasValue("Professor");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.RespFinanceiro", b =>
                {
                    b.HasBaseType("HumanMusicSchoolManager.Models.Models.Pessoa");


                    b.ToTable("RespFinanceiro");

                    b.HasDiscriminator().HasValue("RespFinanceiro");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.ApplicationUser", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.Pessoa", "Pessoa")
                        .WithMany()
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.CursoProfessor", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.Curso", "Curso")
                        .WithMany("Professores")
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanMusicSchoolManager.Models.Models.Professor", "Professor")
                        .WithMany("Cursos")
                        .HasForeignKey("ProfessorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.CursoSala", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.Curso", "Curso")
                        .WithMany("Salas")
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanMusicSchoolManager.Models.Models.Sala", "Sala")
                        .WithMany("Cursos")
                        .HasForeignKey("SalaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.DiarioClasse", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.Matricula", "Matricula")
                        .WithMany()
                        .HasForeignKey("MatriculaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.DispSala", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.Professor", "Professor")
                        .WithMany()
                        .HasForeignKey("ProfessorId");

                    b.HasOne("HumanMusicSchoolManager.Models.Models.Sala", "Sala")
                        .WithMany("DispSalas")
                        .HasForeignKey("SalaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Matricula", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.Aluno", "Aluno")
                        .WithMany("Matriculas")
                        .HasForeignKey("AlunoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanMusicSchoolManager.Models.Models.Curso", "Curso")
                        .WithMany()
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanMusicSchoolManager.Models.Models.Professor", "Professor")
                        .WithMany()
                        .HasForeignKey("ProfessorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("HumanMusicSchoolManager.Models.Models.RespFinanceiro")
                        .WithMany("Matriculas")
                        .HasForeignKey("RespFinanceiroId");
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Pessoa", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HumanMusicSchoolManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HumanMusicSchoolManager.Models.Models.Aluno", b =>
                {
                    b.HasOne("HumanMusicSchoolManager.Models.Models.RespFinanceiro", "RespFinanceiro")
                        .WithMany()
                        .HasForeignKey("RespFinanceiroId");
                });
#pragma warning restore 612, 618
        }
    }
}
