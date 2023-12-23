#pragma warning disable ASP0014

namespace TrickingLibrary.Api;

public static class Program
{
    private const string AllCors = "All";
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.AddControllers();

        services.AddSingleton<TrickyStore>();
        
        services.AddCors(options => options.AddPolicy(
            AllCors, 
            build => build
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));
        
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