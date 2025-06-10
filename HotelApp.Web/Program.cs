using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace HotelApp.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();

        string dbChoice = builder.Configuration
            .GetValue<string>("DatabaseChoice")
            .ToLower();

        switch (dbChoice)
        {
            case "sqlserver":
                builder.Services.AddTransient<IDatabaseData, SqlData>();
                break;
            case "sqlite":
                builder.Services.AddTransient<IDatabaseData, SqliteData>();
                break;
            default:
                throw new InvalidConfigurationException("The DatabaseChoice configuration is not valid.");
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
            .WithStaticAssets();

        app.Run();
    }
}