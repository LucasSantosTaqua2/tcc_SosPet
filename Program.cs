using Microsoft.EntityFrameworkCore;
using SOSPets.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Contexto>
    (options => options.UseSqlServer("server=LUCAS-NOTE; Database=DB_SOS_PETS;Integrated Security=SSPI;TrustServerCertificate=True"));

builder.Services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", options =>
{
    options.Cookie.Name = "UserLoginCookie";
    options.LoginPath = "/Usuario/Login";
    options.AccessDeniedPath = "/Usuario/Erro";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});

var app = builder.Build();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
