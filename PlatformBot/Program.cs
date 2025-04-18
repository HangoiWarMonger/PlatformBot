using Microsoft.EntityFrameworkCore;
using PlatformBot.Infrastructure.DAL.Implementations;
using PlatformBot.Infrastructure.DAL.Implementations.Extensions;
using PlatformBot.Infrastructure.Extensions;
using PlatformBot.Infrastructure.Services.Discord;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDal(builder.Configuration)
    .AddDiscordServices(builder.Configuration)
    .AddHostedService<DiscordBotHostedService>();

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

await app.RunAsync();