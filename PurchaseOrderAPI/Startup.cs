using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PartTracking.Context.Models.Models;
using PartTracking.Service.Repository;
using PartTracking.Service.Service;
using PartTracking.Service.UOfW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrderAPI
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
            services.AddControllers();

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

            #region Context
            services.AddDbContext<PartMgtContext>(options =>
                    options.UseSqlServer(
                      Configuration.GetConnectionString("PartMgtConnection"),
                      b => b.MigrationsAssembly(typeof(PartMgtContext).Assembly.FullName)));
            #endregion

            #region cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            #endregion        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
