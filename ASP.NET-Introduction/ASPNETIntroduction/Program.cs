var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//it takes the connection string with key: 'DefaultConnection' from appsettings.json
builder.Configuration.GetConnectionString("DefaultConnection");



//CONFIGURE DI and Services (order does not matter) till here we configure DI
var app = builder.Build();
//---------------------------------------
//Configure middlewares and filters(order does matter) 

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
