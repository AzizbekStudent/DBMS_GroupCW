
// builder initialization
using FastFood_CW.DAL.Interface;
using FastFood_CW.DAL.Models;
using FastFood_CW.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);
string? _connStr = builder.Configuration.GetConnectionString("DBMS_FastFood_CW")
        .Replace("|DataDirectory|", builder.Environment.ContentRootPath);

// Repository initialization
builder.Services.AddScoped<IRepository<Employee>>(
   setting =>
   {
       return new EmployeeDapperRepository(_connStr);
   });

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
