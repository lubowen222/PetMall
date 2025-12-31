using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Offline.PetMall.WinServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            Console.WriteLine("******Main**********");
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
               Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                 .UseWindowsService(options =>
                 {
                     options.ServiceName = "SpiderWinServer";
                 })
                .UseAutofac()
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddLog4Net($"{AppContext.BaseDirectory}/log4net.config");
                })
                .ConfigureServices(services =>
                {
                    services.AddApplication<WinServiceModule>();
                });
    }
}
