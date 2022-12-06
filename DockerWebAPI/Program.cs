using System.Net;
using Amazon;
using Amazon.S3;
using DockerWebAPI;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.Aws.S3;
using Serilog;
using Unleash;
using Unleash.ClientFactory;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext().Enrich.WithProperty("appName", "sampleapp")
    .CreateLogger();

builder.Logging.AddSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUnleash>(x =>
{
    var settings = new UnleashSettings()
    {
        AppName = "dot-net-client",
        ProjectId = "TrialProject",
        UnleashApi = new Uri("https://eu.app.unleash-hosted.com/eubb7011/api/"),
        CustomHttpHeaders = new Dictionary<string,string>()
        {
            { "Authorization", builder.Configuration[Consts.ApiKey] }
        }
    };

    return new DefaultUnleash(settings);
});
builder.Services.AddHealthChecksUI(x =>
{
    x.AddHealthCheckEndpoint("default api", "/healthz"); //map health check api
}).AddInMemoryStorage();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.UseRouting();
app.Run();