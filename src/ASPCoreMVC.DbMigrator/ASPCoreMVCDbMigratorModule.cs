using ASPCoreMVC.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace ASPCoreMVC.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(ASPCoreMVCEntityFrameworkCoreDbMigrationsModule),
        typeof(ASPCoreMVCApplicationContractsModule)
        )]
    public class ASPCoreMVCDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
