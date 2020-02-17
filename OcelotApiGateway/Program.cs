using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;

namespace OcelotApiGateway
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    new WebHostBuilder()
        //    .UseKestrel()
        //    .UseContentRoot(Directory.GetCurrentDirectory())
        //    .UseStartup<Startup>()
        //    .ConfigureAppConfiguration((hostingContext, config) =>
        //    {
        //        config
        //            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        //            .AddJsonFile("ocelot.json")
        //            .AddEnvironmentVariables();
        //    })
        //    //.ConfigureServices(s => {
        //    //    s.AddOcelot();
        //    //})
        //    //.ConfigureLogging((hostingContext, logging) =>
        //    //{
        //    //       //add your logging
        //    //   })
        //    //.UseIISIntegration()
        //    //.Configure(app =>
        //    //{
        //    //    app.UseOcelot().Wait();
        //    //})
        //    .Build()
        //    .Run();
        //}

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("ocelot.json")
                    .AddEnvironmentVariables();
            });
    }
}
