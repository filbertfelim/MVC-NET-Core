using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MvcBook.Data;
using MvcBook.Models;
using MvcBook.Repositories;
using MvcBook.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddDbContext<BookContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepository<Author>, AuthorRepository>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<IRepository<Book>, BookRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BookContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
