using System.Threading.Tasks;

namespace IoT.Data
{
    public interface IIoTDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
