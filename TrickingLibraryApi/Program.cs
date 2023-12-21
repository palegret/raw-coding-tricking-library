#pragma warning disable ASP0014

namespace TrickingLibraryApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseEndpoints(endpoints => {
            endpoints.MapDefaultControllerRoute();
        });
        
        app.Run();
    }
}