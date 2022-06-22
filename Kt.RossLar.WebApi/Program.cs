using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using System;

using System.IO;

using System.Reflection;
using System.Runtime.Loader;
using Kt.RossLar.WebApi.Regisger;

namespace Kt.RossLar.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TryLoadAssembly();
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.config"));            
            //ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            //BasicConfigurator.Configure(repository);
            //ILog log = LogManager.GetLogger(repository.Name, "NETCorelog4net");
            
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            ILog log = LogManager.GetLogger(repository.Name, "NETCorelog4net");
            
 
            RegAndStartup.Reg();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureAppConfiguration(cfg =>
                    {
                        cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    }).ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.AllowSynchronousIO = true;//ÆôÓÃÍ¬²½ IO                        
                    })
                    .ConfigureAppConfiguration(builder =>
                    {
                        builder.AddJsonFile("hosting.json", optional: true);
                    });
                    //.UseContentRoot(Directory.GetCurrentDirectory())
                    //.UseWebRoot(Directory.GetCurrentDirectory() + @"\wwwroot\");
                });        
    }
}
