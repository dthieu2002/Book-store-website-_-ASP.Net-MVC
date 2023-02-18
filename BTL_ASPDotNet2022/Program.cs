using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BTL_ASPDotNet2022.Data;
using BTL_ASPDotNet2022.Models;
using RazorPagesMovie.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BTL_ASPDotNet2022Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BTL_ASPDotNet2022Context") ?? throw new InvalidOperationException("Connection string 'BTL_ASPDotNet2022Context' not found.")));

//=== Add services to the container.
builder.Services.AddControllersWithViews();

//=== Add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

//=== Seed demo account if not exists records
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    AccountSeeder.Initialize(services);    // Seed demo data for account
    BookSeeder.Initialize(services);      // Seed demo data for book
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

/* OLD Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DTHBookStore}/{action=Index}/{id?}");

app.Run();
