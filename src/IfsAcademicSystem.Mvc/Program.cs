// Adicione estes `usings` no topo para ter acesso �s classes necess�rias
using IfsAcademicSystem.Mvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace IfsAcademicSystem.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- IN�CIO DAS ADI��ES DO TUTORIAL (Parte do antigo Startup.cs) ---

            // 1. Registrar o SchoolContext com a string de conex�o do appsettings.json
            builder.Services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

            // 2. Adicionar o filtro de exce��o de banco de dados para o ambiente de desenvolvimento
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // --- FIM DAS ADI��ES ---

            // Adiciona os servi�os para MVC (Controllers e Views)
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configura o pipeline de requisi��es HTTP.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // --- BLOCO PARA POPULAR O BANCO DE DADOS (Seeding) ---
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    // O m�todo Initialize j� chama EnsureCreated ou voc� usar� Migrations.
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Um erro ocorreu ao popular o banco de dados (seeding).");
                }
            }
            // --- FIM DO BLOCO ---

            app.Run();
        }
    }
}