using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ASPCoreMVC.Data;
using Volo.Abp.DependencyInjection;

namespace ASPCoreMVC.EntityFrameworkCore
{
    public class EntityFrameworkCoreASPCoreMVCDbSchemaMigrator
        : IASPCoreMVCDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreASPCoreMVCDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the ASPCoreMVCMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<ASPCoreMVCMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}