using Autofac;
using EasyFrame.AutoMapper;
using EasyFrame.ElasticSearch;
using EasyFrame.Extensions;
using EasyFrame.Lucenes;
using Microsoft.Extensions.DependencyInjection;
using Offline.PetMall.Application.Contracts;
using Offline.PetMall.Domain.Shared;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace Offline.PetMall.Application
{
    [DependsOn(
       typeof(AbpAspNetCoreModule),
       typeof(EasyFrameLucenesModule),
       typeof(PetMallDomainSharedModule),
        typeof(EasyFrameElasticSearchModule)
       )]
    public class PetMallApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = context.Services.GetConfiguration();

            services.AddAutoMapper(new AutomapperProfile());
            services.AddHttpClient();

            var builder = services.GetContainerBuilder();
            builder.RegisterModule(new AutofacModuleRegister(typeof(PetMallApplicationContractsModule).Assembly));
            builder.RegisterModule(new AutofacModuleRegister(typeof(PetMallApplicationModule).Assembly));
        }

    }
}
