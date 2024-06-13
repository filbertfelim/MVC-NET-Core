using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvcBook.Data;
using MvcBook.Models;
using MvcBook.Repositories;
using MvcBook.Services;

public class Startup
{
    public IConfiguration Configuration { get; }
    private readonly ILogger<Startup> _logger;

    public Startup(IConfiguration configuration, ILogger<Startup> logger)
    {
        Configuration = configuration;
        _logger = logger;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        _logger.LogInformation("Configuring services...");

        services.AddDbContext<BookContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        _logger.LogInformation("Added DbContext.");

        services.AddScoped<IRepository<Author>, AuthorRepository>();
        _logger.LogInformation("Added AuthorRepository.");

        services.AddScoped<AuthorService>();
        _logger.LogInformation("Added AuthorService.");

        services.AddControllers();
        _logger.LogInformation("Added Controllers.");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
