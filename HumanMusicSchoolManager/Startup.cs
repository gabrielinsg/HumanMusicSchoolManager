using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;

namespace HumanMusicSchoolManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequiredUniqueChars = 2;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProfessorService, ProfessorService>();
            services.AddTransient<ICursoService, CursoService>();
            services.AddTransient<IAlunoService, AlunoService>();
            services.AddTransient<IMatriculaService, MatriculaService>();
            services.AddTransient<IPessoaService, PessoaService>();
            services.AddTransient<IFuncionarioService, FuncionarioService>();
            services.AddTransient<ISalaService, SalaService>();
            services.AddTransient<IPacoteAulaService, PacoteAulaService>();
            services.AddTransient<IRespFinanceiroService, RespFinanceiroService>();
            services.AddTransient<IPacoteCompraService, PacoteCompraService>();
            services.AddTransient<IDispSalaService, DispSalaService>();
            services.AddTransient<ITaxaMatriculaService, TaxaMatriculaService>();
            services.AddTransient<IFinanceiroService, FinanceiroService>();
            services.AddTransient<IFeriadoService, FeriadoService>();
            services.AddTransient<IEventoService, EventoService>();
            services.AddTransient<IAulaService, AulaService>();
            services.AddTransient<IChamadaService, ChamadaService>();
            services.AddTransient<IReposicaoService, ReposicaoService>();
            services.AddTransient<IContratoService, ContratoService>();
            services.AddTransient<ICandidatoService, CandidatoService>();
            services.AddTransient<IDemostrativaService, DemostrativaService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(service).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var pessoaService = serviceProvider.GetRequiredService<IPessoaService>();

            IdentityResult roleResult;
            //Adding Admin Role

            string[] roles = new string[] { "Admin", "Professor", "Financeiro", "Coordenacao", "Diretoria", "Financeiro", "Secretaria", "Atendimento", "Vendas" };
            //create the roles and seed them to the database

            foreach (var role in roles)
            {
                if (!await RoleManager.RoleExistsAsync(role))
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(role));
                }
            }

            ApplicationUser UserResult = await UserManager.FindByEmailAsync("admin@admin");

            if (UserResult == null)
            {

                Pessoa pessoa = new Pessoa()
                {
                    Nome = "Administrador",
                    Ativo = true,
                    Email = "admin@admin",
                    CPF = "",
                    RG  = "",
                    Endereco = new Endereco()
                    {
                        Bairro = "Bairro",
                        CEP = "11111-111",
                        Cidade = "Cidade",
                        Logradouro = "Logradouro",
                        Numero = 1234,
                        UF = UF.SP
                    }
                };

                var pessoaSalva = pessoaService.Cadastrar(pessoa);

                var user = new ApplicationUser { UserName = "admin.admin", Email = "admin@admin", PessoaId = pessoaSalva.Id.Value };
                var result = await UserManager.CreateAsync(user, "Admin@admin456");
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, "Admin");
                }
            }
            ////Assign Admin role to the main User here we have given our newly registered 
            ////login id for Admin management
            //ApplicationUser user = await UserManager.FindByEmailAsync("syedshanumcain@gmail.com");
            //var User = new ApplicationUser();
            //await UserManager.AddToRoleAsync(user, "Admin");
        }
    }
}
