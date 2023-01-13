using Zarinpal.AspNetCore.Extensions;
using Zarinpal.AspNetCore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Bind From 'appsettings.json'
builder.Services.AddZarinpal(options =>
{
    builder.Configuration.GetSection("Zarinpal").Bind(options);
});

// Or bind it like this
/* builder.Services.AddZarinpal(options =>
{
    options.MerchantId = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx";
    options.ZarinpalMode = ZarinpalMode.Sandbox;
}, true); */

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
