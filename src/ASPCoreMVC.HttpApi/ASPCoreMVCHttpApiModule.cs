using Localization.Resources.AbpUi;
using ASPCoreMVC.Localization;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.TenantManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;

namespace ASPCoreMVC
{
    [DependsOn(
        typeof(ASPCoreMVCApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule)
        )]
    public class ASPCoreMVCHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
            //ConfigureMaxUploadFileSize(context.Services);
        }

        //private void ConfigureMaxUploadFileSize(IServiceCollection services)
        //{
        //    services.Configure<FormOptions>(opt =>
        //    {
        //        opt.MultipartBodyLengthLimit = AppContractConsts.MaxFileSizeInBytes;
        //    });
        //}

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ASPCoreMVCResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
