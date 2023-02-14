using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//json dosyalarýný eklemek için konfigrüsayson
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();
});
builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", x =>
{
    x.Authority = builder.Configuration["IdentityServerURL"];
    x.Authority = "resource_gateway";
    x.RequireHttpsMetadata = false;
});


builder.Services.AddOcelot();



var app = builder.Build();

await app.UseOcelot();


app.MapGet("/", () => "Hello World!");

app.Run();
