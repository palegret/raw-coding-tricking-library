#pragma warning disable ASP0000
#pragma warning disable ASP0014

using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api;

public static class Program
{
    private const string AllCors = "All";
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.AddControllers();

        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Dev"));

        services.AddCors(options => options.AddPolicy(
            AllCors, 
            build => build
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));
     
        var serviceProvider = services.BuildServiceProvider();

        using (var serviceScope = serviceProvider.CreateScope())
        {
            var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            var environment = serviceScope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            if (environment.IsDevelopment())
            {
                appDbContext.Categories.Add(new Category { Id = "kick", Name = "Kick", Description = "A kick." });
                appDbContext.Categories.Add(new Category { Id = "flip", Name = "Flip", Description = "A flip." });
                appDbContext.Categories.Add(new Category { Id = "transition", Name = "Transition", Description = "A transition." });
                appDbContext.Difficulties.Add(new Difficulty { Id = "easy", Name = "Easy", Description = "An easy difficulty trick." });
                appDbContext.Difficulties.Add(new Difficulty { Id = "medium", Name = "Medium", Description = "A medium difficulty trick." });
                appDbContext.Difficulties.Add(new Difficulty { Id = "hard", Name = "Hard", Description = "A hard difficulty trick." });

                appDbContext.Tricks.Add(new Trick {
                    Id = "backwards-roll",
                    Name = "Backwards Roll",
                    Description = "A backwards roll.",
                    Difficulty = "easy",
                    TrickCategories = new List<TrickCategory> {
                        new() { CategoryId = "transition" }
                    }
                });

                appDbContext.Tricks.Add(new Trick {
                    Id = "backflip",
                    Name = "Backflip",
                    Description = "A backflip.",
                    Difficulty = "medium",
                    TrickCategories = new List<TrickCategory> {
                        new() { CategoryId = "flip" }
                    },
                    Prerequisites = new List<TrickRelationship> {
                        new() { PrerequisiteId = "backwards-roll" }
                    }
                });

                appDbContext.SaveChanges();
            }
        }

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseCors(AllCors);
        app.UseRouting();

        app.UseEndpoints(endpoints => {
            endpoints.MapDefaultControllerRoute();
        });
        
        app.Run();
    }
}