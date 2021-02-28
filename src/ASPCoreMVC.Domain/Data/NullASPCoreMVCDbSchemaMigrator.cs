using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ASPCoreMVC.Data
{
    /* This is used if database provider does't define
     * IASPCoreMVCDbSchemaMigrator implementation.
     */
    public class NullASPCoreMVCDbSchemaMigrator : IASPCoreMVCDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}