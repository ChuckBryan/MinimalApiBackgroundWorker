using System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<MyService>();
builder.Services.AddHealthChecks()
                .AddRedis(redisConnectionString: builder.Configuration["ConnectionStrings:Redis"]);

/*
builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});
*/

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();


public class MyService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //do some work
            Console.WriteLine("Hello World!");
            await Task.Delay(1000, stoppingToken);
        }
    }
}