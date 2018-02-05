using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using tabletop.Data;
using tabletop.Interfaces;
using tabletop.Services;
using Microsoft.Extensions.Logging;
using tabletop.Handlers;
using tabletop.Services.WebSocketManager;


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


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(GetConnectionString()));
            services.AddScoped<IUpdate, SqlUpdateStatus>();

            services.AddWebSocketManager();

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILogger<Startup> logger,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // https://stackoverflow.com/questions/45311393/asp-net-core-reverse-proxy-with-different-root
            app.UsePathBase("/tabletop");

            app.UseStatusCodePages("text/html", "Status code page, status code: {0}");

            app.UseWebSockets();

            app.MapWebSocketManager("/socket", serviceProvider.GetService<NotificationsHandler>());


            app.UseMvc(ConfigureRoutes);


            //app.UseDefaultFiles();
            app.UseStaticFiles();



            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }

        private static void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            // Home/Index/4 > HomeController
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }

    }

}
