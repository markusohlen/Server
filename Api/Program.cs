using Api.Extensions;
using Database.EfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<MealPlannerDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.RegisterServices(builder.Configuration);
        builder.Services.RegisterRepositories();

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1024L * 1024L * 1024L; // 1 GB
        });

        //CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost3000",
                policy =>
                {
                    policy.WithOrigins(
                               "http://localhost:3000",
                               "https://localhost:7093",
                               "http://localhost:5078")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        var app = builder.Build();

        // Use CORS
        app.UseCors("AllowLocalhost3000");

        // Apply migrations
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<MealPlannerDbContext>();
            context.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}