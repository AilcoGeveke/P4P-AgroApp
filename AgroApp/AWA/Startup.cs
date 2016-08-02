using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWA.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AWA
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = @"Server=tcp:agrodatabase.database.windows.net,1433;Initial Catalog=agrodatabase;Persist Security Info=False;User ID=agrouser;Password=AgroPass01;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<AgroContext>(options => options.UseSqlServer(connection));

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            // Add framework services.
            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AccessDeniedPath = new PathString("/notfound"),
                AutomaticAuthenticate = true
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
