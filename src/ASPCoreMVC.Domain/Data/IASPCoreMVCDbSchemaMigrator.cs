using System.Threading.Tasks;

namespace ASPCoreMVC.Data
{
    public interface IASPCoreMVCDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
