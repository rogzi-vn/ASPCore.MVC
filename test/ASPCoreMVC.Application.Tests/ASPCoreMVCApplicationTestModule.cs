using Volo.Abp.Modularity;

namespace ASPCoreMVC
{
    [DependsOn(
        typeof(ASPCoreMVCApplicationModule),
        typeof(ASPCoreMVCDomainTestModule)
        )]
    public class ASPCoreMVCApplicationTestModule : AbpModule
    {

    }
}