using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProyectoServiciosWeb.Models;

namespace ProyectoServiciosWeb
{

    public static class DatabaseContextConfigurator
    {
        public static void ConfigureDatabaseContexts(WebApplicationBuilder builder)
        {
            ConfigureDbContext<VueloItemcs>(builder, "DefaultConnection");
            ConfigureDbContext<AerolineaItem>(builder, "DefaultConnection");
            ConfigureDbContext<PaisesItem>(builder, "DefaultConnection");
            ConfigureDbContext<PivoteItem>(builder, "DefaultConnection");
            ConfigureDbContext<PreguntaItem>(builder, "DefaultConnection");
            ConfigureDbContext<PuertasItem>(builder, "DefaultConnection");
            ConfigureDbContext<UsuarioItem>(builder, "DefaultConnection");
            ConfigureDbContext<RolesItem>(builder, "DefaultConnection");
            ConfigureDbContext<BitacoraItem>(builder, "DefaultConnection");
            ConfigureDbContext<TarjetaUsuarioItem>(builder, "DefaultConnection");
            ConfigureDbContext<Tipo_VueloItem>(builder, "DefaultConnection");
            ConfigureDbContext<ReservacionesItem>(builder, "DefaultConnection");
        }

        private static void ConfigureDbContext<TContext>(WebApplicationBuilder builder, string connectionName)
            where TContext : DbContext
        {
            var connectionString = builder.Configuration.GetConnectionString(connectionName);
            builder.Services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DatabaseContextConfigurator.ConfigureDatabaseContexts(builder); ;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("NuevaPolitica", app =>
                {
                    app.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("NuevaPolitica");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}