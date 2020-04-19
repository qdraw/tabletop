using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Services;
using Microsoft.Extensions.Logging;
using tabletop.Hubs;
using tabletop.Models;


namespace tabletop
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable("TABLETOP_SQL");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine(">> connectionString from .json file");

                connectionString = _configuration.GetConnectionString("DefaultConnection");
            }
            return connectionString;
        }

        private AppSettings.DatabaseTypeList GetConnectionType()
        {
	        var databaseTypeString = Environment.GetEnvironmentVariable("TABLETOP_DATABASETYPE");
	        if ( string.IsNullOrWhiteSpace(databaseTypeString) )
	        {
		        Console.WriteLine(">> databaseTypeString from .json file");
		        databaseTypeString = _configuration.GetConnectionString("DatabaseType");
	        }
	        
	        var result = Enum.TryParse<AppSettings.DatabaseTypeList>(databaseTypeString, 
		        true, out var databaseType);
	        return result ? databaseType : AppSettings.DatabaseTypeList.Sqlite;
        }

        public void ConfigureServices(IServiceCollection services)
        {
	        var connectionType = GetConnectionType();
	        
            switch (connectionType)
            {
	            case AppSettings.DatabaseTypeList.SqlServer:
		            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(GetConnectionString()));
		            break;
	            case AppSettings.DatabaseTypeList.Mysql:
		            services.AddDbContext<AppDbContext>(
			            options => options.UseMySql(GetConnectionString()));
		            break;
	            case AppSettings.DatabaseTypeList.InMemoryDatabase:
		            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("starsky"));
		            break;
	            case AppSettings.DatabaseTypeList.Sqlite:
		            var sqLitePath = new AppSettings().SqLiteFullPath(
			            AppSettings.DatabaseTypeList.Sqlite,
			            GetConnectionString(),
			            AppDomain.CurrentDomain.BaseDirectory);
	                services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqLitePath));
		            break;
            }
            
            services.AddScoped<IUpdate, SqlUpdateStatus>();
            services.AddSignalR();

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            });
            services.AddMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // https://stackoverflow.com/questions/45311393/asp-net-core-reverse-proxy-with-different-root
            app.UsePathBase("/tabletop");

            app.UseStatusCodePages("text/html", "Status code page, status code: {0}");
           
            app.UseRouting();
	        app.UseEndpoints(ConfigureRoutes);
	        
            app.UseStaticFiles();
            
            EfCoreMigrationsOnProject(app);
        }

        private static void ConfigureRoutes(IEndpointRouteBuilder routeBuilder)
        {
            // Home/Index/4 > HomeController
            routeBuilder.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            routeBuilder.MapHub<DataHub>("/datahub");
        }
        
        /// <summary>
        /// Run the latest migration on the database. 
        /// To start over with a SQLite database please remove it and
        /// it will add a new one
        /// </summary>
        private void EfCoreMigrationsOnProject(IApplicationBuilder app)
        {
	        try
	        {
		        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
			        .CreateScope();
		        var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
		        dbContext.Database.Migrate();
	        }
	        catch (MySql.Data.MySqlClient.MySqlException e)
	        {
		        Console.WriteLine(e);
	        }
        }

    }

}
