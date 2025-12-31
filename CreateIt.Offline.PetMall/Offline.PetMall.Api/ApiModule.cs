using EasyFrame.ConfigCenter;
using EasyFrame.CurrentUser;
using EasyFrame.Extensions;
using EasyFrame.Jwt;
using EasyFrame.Mongo;
using EasyFrame.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Offline.PetMall.Application;
using Offline.PetMall.Application.Contracts;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Offline.PetMall.Api
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpAutofacModule),
        typeof(EasyFrameRedisModule),
        typeof(EasyFrameMongoModule),
        typeof(EasyFrameJwtModule),
        typeof(EasyFrameCurrentUserModule),
        typeof(EasyFrameConfigCenterModule),
        typeof(PetMallApplicationContractsModule),
        typeof(PetMallApplicationModule)
        )]
    public class PetMallApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = context.Services.GetConfiguration();

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionAttribute>();

                //拦截反伪造请求(跨站请求伪造)
                //options.Filters.Add((new AutoValidateAntiforgeryTokenAttribute()));

                //取消Abp禁用反伪造功能
                options.Filters.Add((new IgnoreAntiforgeryTokenAttribute()));
            }).AddNewtonsoftJson(ops =>
            {
                ops.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                ops.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ops.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            services.AddModelValidate();
            services.AddSwagger(typeof(Startup).Assembly, "供应商管理");
            services.AddCors(configuration);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Offline.Sup");
            });

            app.UseCors();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            context.InitConfigCenter();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            base.OnApplicationShutdown(context);
        }
    }
}
