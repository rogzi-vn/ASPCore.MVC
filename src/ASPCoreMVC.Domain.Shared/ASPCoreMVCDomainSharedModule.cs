using ASPCoreMVC.Localization;
using System.IO;
using System.Reflection;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace ASPCoreMVC
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpBackgroundJobsDomainSharedModule),
        typeof(AbpFeatureManagementDomainSharedModule),
        typeof(AbpIdentityDomainSharedModule),
        typeof(AbpIdentityServerDomainSharedModule),
        typeof(AbpPermissionManagementDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(AbpTenantManagementDomainSharedModule)
        )]
    public class ASPCoreMVCDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            ASPCoreMVCGlobalFeatureConfigurator.Configure();
            ASPCoreMVCModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Lấy đường dẫn của thư mục tài nguyên công khai
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Path.Combine(path, "wwwroot");

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                // Đọc tài nguyên từ file ảo
                options.FileSets.AddEmbedded<ASPCoreMVCDomainSharedModule>();

                // Nếu tông tại, Thay thế tệp tảo thành thật (tệp vật lý)
                if (Directory.Exists(Path.Combine(path, "Localization", "ASPCoreMVC")))
                    options.FileSets.ReplaceEmbeddedByPhysical<ASPCoreMVCDomainSharedModule>(path);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ASPCoreMVCResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/ASPCoreMVC");

                options.DefaultResourceType = typeof(ASPCoreMVCResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("ASPCoreMVC", typeof(ASPCoreMVCResource));
            });
        }
    }
}
