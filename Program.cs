using Microsoft.EntityFrameworkCore;
using SpecialistTestApp1.Calculators;
using SpecialistTestApp1.DataContext;
using SpecialistTestApp1.Services;
using System.Reflection.Metadata.Ecma335;

namespace SpecialistTestApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("TestDataContext") ?? throw new InvalidOperationException("Connection string 'TestDataContext' not found.");

			builder.Services.AddDbContext<TestDataContext>(options => options.UseInMemoryDatabase("TestDatabase"));
			// Add services to the container.

			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
			builder.Services.AddTransient<IOrderCalculator, OrderCalculator>();
			builder.Services.AddScoped<IEmailService, EmailService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


			app.MapControllers();

			app.Run();
        }
    }
}
