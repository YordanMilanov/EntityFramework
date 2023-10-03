
using ForumApp.Data;
using Forum.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Forum.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add dpContext / repositories to the IoC container
builder.Services.AddDbContext<ForumDbContext>(options =>
options.UseSqlServer(connectionString));

//Add custom services / bussiness layer
//here we describe that everytime we ask for IPostService, it will inject PostService the class not the Interface
builder.Services.AddScoped<IPostService, PostService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
