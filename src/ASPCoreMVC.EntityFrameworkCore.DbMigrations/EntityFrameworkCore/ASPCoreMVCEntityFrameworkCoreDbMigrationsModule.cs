using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ASPCoreMVC.EntityFrameworkCore
{
    [DependsOn(
        typeof(ASPCoreMVCEntityFrameworkCoreModule)
        )]
    public class ASPCoreMVCEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ASPCoreMVCMigrationsDbContext>();
        }
    }
}
