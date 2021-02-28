using ASPCoreMVC.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ASPCoreMVC
{
    [DependsOn(
        typeof(ASPCoreMVCEntityFrameworkCoreTestModule)
        )]
    public class ASPCoreMVCDomainTestModule : AbpModule
    {

    }
}