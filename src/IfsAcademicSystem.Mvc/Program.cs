// Adicione estes `usings` no topo para ter acesso às classes necessárias
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

            // --- INÍCIO DAS ADIÇÕES DO TUTORIAL (Parte do antigo Startup.cs) ---

            // 1. Registrar o SchoolContext com a string de conexão do appsettings.json
            builder.Services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

            // 2. Adicionar o filtro de exceção de banco de dados para o ambiente de desenvolvimento
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // --- FIM DAS ADIÇÕES ---

            // Adiciona os serviços para MVC (Controllers e Views)
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configura o pipeline de requisições HTTP.
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
                    // O método Initialize já chama EnsureCreated ou você usará Migrations.
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