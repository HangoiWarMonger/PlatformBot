using Microsoft.EntityFrameworkCore;
using PlatformBot.Common.Extensions;
using PlatformBot.Common.Services.Discord;
using PlatformBot.Features.MergeRequestRedirect.Services;
using PlatformBot.Infrastructure.DAL.Implementations;
using PlatformBot.Infrastructure.DAL.Implementations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services
    .AddDal(builder.Configuration)
    .AddDiscordServices(builder.Configuration)
    .AddGitLabApi(builder.Configuration)
    .AddHostedService<DiscordBotHostedService>()
    .AddScoped<MrRedirectionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}

app.MapControllers();

await app.RunAsync();