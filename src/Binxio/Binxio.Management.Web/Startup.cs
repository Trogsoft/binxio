using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binxio.Abstractions;
using Binxio.Data;
using Binxio.Management.Web.Services;
using Binxio.Management.Web.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Binxio.Management.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddDbContext<BinxioDb>(opts =>
            //{
            //    opts.UseSqlServer(Configuration.GetConnectionString("binxio"));
            //});

            services.AddHostedService<WebSocketService>();

            services.AddSingleton<ITaskManager, TaskManager>();
            services.AddSingleton<WebSocketMessenger>();

            services.AddTransient<IProjectManager, ProjectManager>();
            services.AddTransient<ITaskTracker, TaskTracker>();

            AddTasks(services);

        }

        private void AddTasks(IServiceCollection services)
        {

            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x=>x.GetTypes().Where(y=>typeof(ITaskBase).IsAssignableFrom(y) && !y.IsAbstract && !y.IsInterface && y.IsPublic)))
            {
                services.AddTransient(type);
            }

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}