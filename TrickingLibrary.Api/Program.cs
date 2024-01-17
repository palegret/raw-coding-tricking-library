#pragma warning disable ASP0014

using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.BackgroundServices;
using TrickingLibrary.Api.BackgroundServices.Messages;
using TrickingLibrary.Api.Helpers;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api;

public static class Program
{
    private const string AllCors = "All";
    private const string DevDbName = "Dev";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.AddControllers();
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(DevDbName));
        services.AddHostedService<VideoEditingBackgroundService>();
        services.AddSingleton(_ => Channel.CreateUnbounded<EditVideoMessage>());
        services.AddSingleton<VideoHelper>();

        services.AddCors(options => options.AddPolicy(
            AllCors, 
            build => build
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));
     
        SeedDevDatabase(services);
        
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

    private static void SeedDevDatabase(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        using var serviceScope = serviceProvider.CreateScope();

        var environment = serviceScope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        if (!environment.IsDevelopment())
            return;

        var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

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
                new() { CategoryId = "flip" },
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

        appDbContext.Submissions.Add(new Submission {
            TrickId = "backflip",
            Description = "A backflip, trying to go for MAX height.",
            Video = new Video {
                VideoLink = "conv_test_video_01.mp4",
                ThumbLink = "thumb_test_video_01.jpg",
            },
			VideoProcessed = true,
        });

        appDbContext.Submissions.Add(new Submission {
            TrickId = "backflip",
            Description = "A backflip, trying to go for MIN height.",
			Video = new Video {
				VideoLink = "conv_test_video_02.mp4",
				ThumbLink = "thumb_test_video_02.jpg",
			},
			VideoProcessed = true,
        });

        appDbContext.SaveChanges();
    }
}