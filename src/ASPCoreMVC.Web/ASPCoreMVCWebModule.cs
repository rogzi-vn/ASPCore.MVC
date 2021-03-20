using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ASPCoreMVC.EntityFrameworkCore;
using ASPCoreMVC.Localization;
using ASPCoreMVC.MultiTenancy;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Microsoft.AspNetCore.Http.Features;
using ASPCoreMVC.Helpers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ASPCoreMVC.Web.Middleware;
using Volo.Abp.AspNetCore.SignalR;

namespace ASPCoreMVC.Web
{
    [DependsOn(
        typeof(ASPCoreMVCHttpApiModule), // DI Custom HTTP API của mình
        typeof(ASPCoreMVCApplicationModule), // DI tằng Application+Application Contracts
        typeof(ASPCoreMVCEntityFrameworkCoreDbMigrationsModule), // DI thằng EF Migrations
        typeof(AbpAutofacModule), // DI Module DI Resolver ---
        typeof(AbpIdentityWebModule), // --
        typeof(AbpAccountWebIdentityServerModule), // DI IS4
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule), // Cho phép xác thực qua JWT
        typeof(AbpTenantManagementWebModule), // DI Modeul tenant
        typeof(AbpAspNetCoreSerilogModule), // :v Log
        typeof(AbpSwashbuckleModule), // Swagger
        typeof(AbpAspNetCoreSignalRModule)
        )]
    public class ASPCoreMVCWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(ASPCoreMVCResource),
                    typeof(ASPCoreMVCDomainModule).Assembly,
                    typeof(ASPCoreMVCDomainSharedModule).Assembly,
                    typeof(ASPCoreMVCApplicationModule).Assembly,
                    typeof(ASPCoreMVCApplicationContractsModule).Assembly,
                    typeof(ASPCoreMVCWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigMaxUploadSize(context.Services);
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);

            ConfigurePassword(context.Services);
            ConfigureTheme();
            ConfigureDefaultPath(context.Services);

            ConfigureBlobStorage(hostingEnvironment);

            ConfigureSignalR(context.Services);
        }

        private void ConfigureSignalR(IServiceCollection services)
        {
            services.AddSignalR();
        }

        private void ConfigMaxUploadSize(IServiceCollection services)
        {
            if (EnvHelpers.IsRunningInProcessIIS())
            {
                services.Configure<IISServerOptions>(options =>
                {
                    options.MaxRequestBodySize = AppContractConsts.MaxFileSizeInBytes;
                });
            }
            else
            {
                services.Configure<KestrelServerOptions>(options =>
                {
                    options.Limits.MaxRequestBodySize = AppContractConsts.MaxFileSizeInBytes;
                });
            }

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }

        private void ConfigureBlobStorage(IWebHostEnvironment hostingEnvironment)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseFileSystem(fileSystem =>
                    {
                        // Lấy đường dẫn của thư mục tài nguyên công khai
                        //var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        //path = Path.Combine(path, "wwwroot");
                        if (!new DirectoryInfo(hostingEnvironment.WebRootPath).Exists)
                        {
                            Directory.CreateDirectory(hostingEnvironment.WebRootPath);
                        }
                        fileSystem.BasePath = hostingEnvironment.WebRootPath;
                    });
                });
            });
        }

        private void ConfigurePassword(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });
        }


        private void ConfigureDefaultPath(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
            });


            // Chỉnh sửa đường dẫn đăng nhập
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/account/login";
                //config.LogoutPath = "/account/logout";

            });
        }

        private void ConfigureTheme()
        {
            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Add<AppTheme>();
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(options => { })
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "ASPCoreMVC";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ASPCoreMVCWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<ASPCoreMVCDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ASPCoreMVC.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ASPCoreMVCDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ASPCoreMVC.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ASPCoreMVCApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ASPCoreMVC.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ASPCoreMVCApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ASPCoreMVC.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ASPCoreMVCWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English", "226-united-states.svg"));
                options.Languages.Add(new LanguageInfo("vi", "vi", "Việt Nam", "220-vietnam.svg"));
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ASPCoreMVCApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ASPCoreMVC API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseHttpsRedirection();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            app.UseExamContinousMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ASPCoreMVC API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
