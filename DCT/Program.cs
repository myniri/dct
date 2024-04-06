using BLL.Interfaces;
using BLL.Services;
using HackerNewsIntegration.Interfaces;
using HackerNewsIntegration.Options;
using HackerNewsIntegration.Services;
using HttpClientHelper.Interfaces;
using HttpClientHelper.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<HackerNewsIntegrationOptions>(builder.Configuration.GetSection("HackerNewsOptions"));
builder.Services.AddHttpClient<IBaseHttpClient, BaseHttpClient>();
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
builder.Services.AddScoped<IHackerNewsIntegrationService, HackerNewsIntegrationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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