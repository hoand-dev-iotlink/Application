using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace IoT.Data
{
    /* This is used if database provider does't define
     * IIoTDbSchemaMigrator implementation.
     */
    public class NullIoTDbSchemaMigrator : IIoTDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}