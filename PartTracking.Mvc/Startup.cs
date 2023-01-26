using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartTracking.Context.Models.Models;
using PartTracking.Service.Repository;
using PartTracking.Service.Service;
using PartTracking.Service.UOfW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartTracking.Mvc
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

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IOrderMasterRepository, OrderMasterRepository>();
            services.AddTransient<IPartMasterRepository, PartMasterRepository>();
            services.AddTransient<IPartDetailRepository, PartDetailRepository>();
            services.AddTransient<IReceivingRepository, ReceivingRepository>();
            services.AddTransient<ICustomerWorkOrderRepository, CustomerWorkOrderRepository>();
            services.AddTransient<ITrackingRepository, TrackingRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            #region FMSContext
            services.AddDbContext<PartMgtContext>(options =>
                    options.UseSqlServer(
                      Configuration.GetConnectionString("PartMgtConnection"),
                      b => b.MigrationsAssembly(typeof(PartMgtContext).Assembly.FullName)));
            #endregion

            #region Session
            services.AddSession(options =>
            {
                options.Cookie.Name = ".PartMgt.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseStatusCodePages("text/html", "We're <b>really</b> sorry, but something went wrong. Error code: {0}");
            app.UseStatusCodePagesWithRedirects("/Error/Http?statusCode={0}");


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

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
