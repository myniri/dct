using BLL.Interfaces;
using BLL.Services;
using HackerNewsIntegration.Interfaces;
using HackerNewsIntegration.Options;
using HackerNewsIntegration.Services;
using HttpClientHelper.Interfaces;
using HttpClientHelper.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<HackerNewsIntegrationOptions>(builder.Configuration.GetSection("HackerNewsOptions"));
builder.Services.AddHttpClient<IBaseHttpClient, BaseHttpClient>();
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
builder.Services.AddScoped<IHackerNewsIntegrationService, HackerNewsIntegrationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Developer Coding Test",
        Description = @"Bamboo-card - Developer Coding Test
                        stories from the Hacker News API, as determined by their score, where
                        n is specified by the caller to the API.",
    });

    c.EnableAnnotations();
});

builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    serviceScope.ServiceProvider.GetRequiredService<IHackerNewsService>().GetStoriesAsync(1);
}

app.Run();