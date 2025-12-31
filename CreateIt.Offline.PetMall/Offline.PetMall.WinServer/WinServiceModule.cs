using EasyFrame.CurrentUser;
using EasyFrame.Jwt;
using EasyFrame.Redis;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using EasyFrame.ConfigCenter;
using Offline.PetMall.Application;
using Offline.PetMall.Application.Contracts;

namespace Offline.PetMall.WinServer
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpAutofacModule),
        typeof(EasyFrameRedisModule),
        typeof(EasyFrameJwtModule),
        typeof(EasyFrameCurrentUserModule),
        typeof(EasyFrameConfigCenterModule),
        typeof(PetMallApplicationContractsModule),
        typeof(PetMallApplicationModule)
    )]
    public class WinServiceModule : AbpModule
    {
        /// <summary>
        /// service注入
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Console.WriteLine("...service注入");
            var services = context.Services;
            services.AddHostedService<SpiderPolingWorker>();
        }
    }
}
