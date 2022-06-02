using EmailNotificationService.Channels;
using InfrastructureLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EmailNotificationService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.Development.json", optional: false);
                    //config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    AddRabbitMQ(hostContext, services);
                    AddEmailServer(hostContext, services);
                    services.AddHostedService<NotificationManager>();
                })
                .UseConsoleLifetime();

            return host;
        }

        private static void AddEmailServer(HostBuilderContext hostContext, IServiceCollection services)
        {
            var emailServerConfigSection = hostContext.Configuration.GetSection("Email");
            var emailHost = emailServerConfigSection["Host"];
            var emailPort = Convert.ToInt32(emailServerConfigSection["Port"]);
            var userName = emailServerConfigSection["UserName"];
            var password = emailServerConfigSection["Password"];
            services.AddTransient<IEmailNotifier>(en => new EmailNotifier(emailHost, emailPort, userName, password));
        }

        private static void AddRabbitMQ(HostBuilderContext hostContext, IServiceCollection services)
        {
            var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
            var userName = rabbitMQConfigSection["UserName"];
            var password = rabbitMQConfigSection["Password"];
            var host = rabbitMQConfigSection["Host"];
            var exchangeName = rabbitMQConfigSection["ExchangeName"];
            var exchangeType = rabbitMQConfigSection["ExchangeType"];
            var queueName = rabbitMQConfigSection["QueueName"];
            var routingKey = rabbitMQConfigSection["RoutingKey"];
            services.AddTransient<IMessageHandler>((mp) => new RabbitMQMessageHandler(host, userName, password, exchangeName, exchangeType, queueName, routingKey));
        }
    }
}
