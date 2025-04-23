using Microsoft.EntityFrameworkCore;
using Serilog;
using WorkHunterUtils;
using WorkHunterUtils.Data;

namespace CurrencyHistory.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration)
                => loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration));

            builder.Services.RegisterApplicationServices(builder.Configuration);

            builder.Services.AddDbContext<IWorkHunterUtilsDb, WorkHunterUtilsDb>(config =>
            {
                config.UseNpgsql(builder.Configuration.GetConnectionString("WorkHunterUtilsDb"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
