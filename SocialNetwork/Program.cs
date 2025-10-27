using Microsoft.AspNetCore.Identity;
using SocialNetwork.Core.Application;
using SocialNetwork.Infrastructure.Identity;
using SocialNetwork.Infrastructure.Persistence;
using SocialNetwork.Infrastructure.Shared;
using SocialNetwork.Infrastructure.Identity.Entities;
using SocialNetwork.Infrastructure.Identity.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddApplicationServicesIoc();
builder.Services.AddPersistenceServicesIoc(builder.Configuration);
builder.Services.AddSharedServicesIoc(builder.Configuration);
builder.Services.AddIdentityServicesIocWeb(builder.Configuration);

var app = builder.Build();

await app.Services.SeedDefaultUserAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
