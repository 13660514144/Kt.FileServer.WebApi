using EmbeddedWebBrowserSolution;
using HelperTools;
using Kt.RossLar.WebApi.Model;
using Microsoft.Extensions.DependencyInjection;
using RollLar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Kt.RossLar.WebApi.Model.RepModels;

namespace Kt.RossLar.WebApi.Regisger
{
    public class RegAndStartup
    {
        //private static PassRecordJob _PassRecordJob;
        private static ResponseTo _ResponseTo;
        //private static RossLarGate _RossLarGate;
        public static void Reg()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ConFigHelper>();
            services.AddSingleton<TimeSpans>();
            services.AddSingleton<ResponseTo>();
           
            IServiceProviderHelper.ServiceProvider = services.BuildServiceProvider();
        }              
    }
}
