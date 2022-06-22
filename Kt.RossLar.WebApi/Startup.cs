using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperTools;
using EmbeddedWebBrowserSolution;
using Kt.RossLar.WebApi.Model;
using Hangfire;
using Hangfire.MemoryStorage;
using RollLar;
using static Kt.RossLar.WebApi.Model.RepModels;
using System.IO;
using Microsoft.OpenApi.Models;

namespace Kt.RossLar.WebApi
{
    public class Startup
    {
        //private PassRecordJob _PassRecordJob;
        private ResponseTo _ResponseTo;
 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //注意不能用大写V1，不然老报错，Not Found /swagger/v1/swagger.json
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            //services.AddCors();
          
            services.AddHangfire(x => x.UseStorage(new MemoryStorage()));
            services.AddControllers();            
            OnInit();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 开启静态页面
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot")),
                // 下面Linux 使用
                //FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                //    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });
            //_=app.UseMvcWithDefaultRoute();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            // UseSwaggerUI方法调用启用静态文件中间件。

            //app.UseSwaggerUI(c => {
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            //app.UseHangfireDashboard();   //使用hangfire面板
            //_ = app.UseHangfireServer();
            //RecurringJob.AddOrUpdate(() => _PassRecordJob.PassRecord(), "*/2 * * * * *", TimeZoneInfo.Local);

            app.UseRouting();
           
            app.UseAuthorization();
            app.UseCors(builder => builder.WithOrigins().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void OnInit()
        {
            try
            {
                var _ConFigHelper = IServiceProviderHelper.ServiceProvider.GetService(typeof(ConFigHelper)) as ConFigHelper;
                _ConFigHelper.Init();
                _ResponseTo = IServiceProviderHelper.ServiceProvider.GetService(typeof(ResponseTo)) as ResponseTo;
                _ResponseTo.Init();
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog($"初始化错误:{ex.Message}");
                System.Environment.Exit(0);
            }
        }
       
    }
}
